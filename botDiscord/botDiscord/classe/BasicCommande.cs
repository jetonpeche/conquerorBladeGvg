using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System.Text;
using botDiscord.models;
using System.Text.RegularExpressions;
using botDiscord.modelExport;

namespace botDiscord.classe
{
    public class BasicCommande: ModuleBase<SocketCommandContext>
    {
        //private const string URL_API = "http://localhost:5019";
        private const string URL_API = "https://cb-gvg-api.jetonpeche.fr"; 

        private static HttpClient http = new();
        private const string MEDIA_TYPE = "application/json";

        private const string patternDate = @"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))$";

        // OK
        [Command("ping")]
        public async Task Test()
        {
            await Context.Channel.SendMessageAsync($"<@{Context.User.Id}>");
        }

        // OK
        [Command("listerGvG")]
        public async Task Lister()
        {
            string listeString = await http.GetStringAsync($"{URL_API}/gvg/listerViaDiscord/{Context.User.Id}");

            Console.WriteLine(listeString);

            if (listeString != "[]" && listeString != null && listeString.StartsWith("["))
            {
                List<Gvg>? listeGvg = JsonConvert.DeserializeObject<List<Gvg>>(listeString);

                EmbedBuilder embedBuilder = new()
                {
                    Title = "Liste des dates de GvG",
                    Color = new Color(Color.DarkPurple)
                };

                foreach (var element in listeGvg)
                    embedBuilder.AddField(element.Date, $"Participe: {element.estInscrit}");

                await Context.Channel.SendMessageAsync(null, false, embedBuilder.Build());
            }
            else
            {
                await Context.Channel.SendMessageAsync(listeString);
            }
        }

        // OK
        [Command("initMonIdDiscord")]
        public async Task AjouterUtilisateur(string _pseudo = "")
        {
            // pseudo cb = pseudo discord
            if (string.IsNullOrEmpty(_pseudo))
                _pseudo = Context.Message.Author.Username.ToLower().Trim();

            string ok = await http.GetStringAsync($"{URL_API}/compte/initIdDiscord/{_pseudo}/{Context.Message.Author.Id}");

            await Context.Channel.SendMessageAsync(ok);
        }

        // OK
        [Command("SupprimeMoi")]
        public async Task SupprimerUtilisateur()
        {
            string msg;

            string retour = await http.DeleteAsync($"{URL_API}/compte/supprimerViaDiscord/{Context.User.Id}").Result.Content.ReadAsStringAsync();

            bool estSupprimer = bool.Parse(retour);

            if (estSupprimer)
            {
                msg = $"Tu as été supprimé de ma base de donnée: {Context.Message.Author.Mention} bye {new Emoji("👋")} !";
            }
            else
            {
                msg = $"Je ne te trouve pas {Context.Message.Author.Mention} ton id discord n'est pas connu. Utilise la comande: !initMonIdDiscord";
            }

            await Context.Channel.SendMessageAsync(msg);
        }

        // OK
        [Command("ajouterDateGvG")]
        public async Task AjouterDate([Remainder] string _dateString)
        {
            string msg = "";
            bool estBloquer = false;

            _dateString = Regex.Replace(_dateString, @"\s+", string.Empty);

            List<GvgExport> listeGvg = new();
            List<string> listeDateString = new(_dateString.Trim().Split(','));

            string estAdminString = await http.GetStringAsync($"{URL_API}/compte/estAdmin/{Context.User.Id}");
            bool estAdmin = bool.Parse(estAdminString);

            if(!estAdmin)
            {
                await Context.Channel.SendMessageAsync("Tu n'as pas l'autorisation pour accéder à cette commande");
                return;
            }

            foreach (var element in listeDateString)
            {
                if (!Regex.Match(element, patternDate, RegexOptions.None).Success)
                {
                    msg = $"Erreur: la date: {element} doit être au format JJ/MM";
                    estBloquer = true;

                    break;
                }

                string jsonString = JsonConvert.SerializeObject(new { IdDiscord = Context.User.Id.ToString(), Date = element + $"/{DateTime.Now.Year}" });
                HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, MEDIA_TYPE);

                string retour = await http.PostAsync($"{URL_API}/gvg/existe", httpContent).Result.Content.ReadAsStringAsync();

                bool existe = bool.Parse(retour);
                    
                if (!existe)
                    listeGvg.Add(new GvgExport { Date = element + $"/{DateTime.Now.Year}" });
                else
                {
                    msg = $"La date {element}/{DateTime.Now.Year} existe déjà";
                    estBloquer = true;

                    break;
                }
            }
            
            if(!estBloquer)
            {
                string jsonString = JsonConvert.SerializeObject(listeGvg);

                HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, MEDIA_TYPE);

                var retour = await http.PostAsync($"{URL_API}/Gvg/ajouter", httpContent);

                if (retour.IsSuccessStatusCode)
                    msg = listeGvg.Count > 1 ? "Les nouvelles dates ont été programmées" : "La nouvelle date a été programmée";
                else
                    msg = "Une erreur a eu lieu";
            }

            await Context.Channel.SendMessageAsync(msg);
        }

        // si string vide inscrit a la prochaine gvg
        [Command("participerGvG")]
        public async Task Participer(string _dateString = "")
        {
            if (!string.IsNullOrEmpty(_dateString) && !Regex.Match(_dateString, patternDate, RegexOptions.None).Success)
                await Context.Channel.SendMessageAsync($"{Context.User.Mention} Erreur: la date doit être au format JJ/MM");
            else
            {
                string jsonString = JsonConvert.SerializeObject(new { IdDiscord = Context.User.Id.ToString(), Date = _dateString + $"/{DateTime.Now.Year}" });
                HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, MEDIA_TYPE);

                string retour = await http.PostAsync($"{URL_API}/gvg/participerViaDiscord", httpContent).Result.Content.ReadAsStringAsync();

                await Context.Channel.SendMessageAsync($"{Context.User.Mention} {retour}");
            }
        }

        [Command("site")]
        public async Task OuvrirSite()
        {
            await Context.Channel.SendMessageAsync("liens du site: https://conqueror-blade-gvg.jetonpeche.fr/");
        }

        // OK
        [Command("aled")]
        public async Task ListerCmd()
        {
            EmbedBuilder embedBuilder = new()
            {
                Title = "Liste des commandes",
                Color = new Color(Color.DarkPurple)
            };

            embedBuilder.AddField("!ping", "Ping l'utilisateur");
            embedBuilder.AddField("!listerGvG", "Liste les GvGs programmées");
            embedBuilder.AddField("!initMonIdDiscord <pseudo cb>", "Ajoute son Id discord (si pseudo = nom discord rien mettre)");
            embedBuilder.AddField("!SupprimeMoi", "Supprime l'utilisateur de la base de donnée");
            embedBuilder.AddField("!ajouterDateGvG <JJ/MM> ou <JJ/MM, JJ/MM...>", "Ajout d'une ou des nouvelles dates de GvG");
            embedBuilder.AddField("!participerGvG <JJ/MM>", "Inscrit l'utilisateur pour la GvG choisi");
            embedBuilder.AddField("!site", "Affiche url du site");
            embedBuilder.AddField("!aled", "Liste des commandes disponibles");

            await Context.Channel.SendMessageAsync(null, false, embedBuilder.Build());
        }

        // [Remainder] => parametre qui accepte les espaces (exemple: salut sa va)
        // plusieur arguments => plusieur parametre de fonction
    }
}
