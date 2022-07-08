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
        public static HttpClient http { get; } = new();
        public static string MEDIA_TYPE { get; } = "application/json";

        private const string patternDate = @"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))$";

        // OK
        [Command("ping")]
        public async Task Test()
        {
            await Context.Channel.SendMessageAsync($"<@{Context.User.Id}>");
        }

        #region commande admin

        [Command("ajouterGvG")]
        public async Task AjouterDate([Remainder] string _dateString)
        {
            string msg = "";

            _dateString = Regex.Replace(_dateString, @"\s+", string.Empty);

            List<GvgExport> listeGvg = new();
            List<string> listeDateString = new(_dateString.Trim().Split(','));

            Outil outil = new();

            if (!await outil.EstAdmin(Context.User.Id.ToString()))
            {
                await Context.Channel.SendMessageAsync("Tu n'as pas l'autorisation pour accéder à cette commande");
                return;
            }

            foreach (var element in listeDateString)
            {
                if (!Regex.Match(element, patternDate, RegexOptions.None).Success)
                {
                    msg = $"Erreur: la date: {element} doit être au format JJ/MM";

                    break;
                }

                if (!await outil.GvgExiste(element))
                    listeGvg.Add(new GvgExport { Date = element + $"/{DateTime.Now.Year}" });
            }

            if (listeGvg.Count > 0)
            {
                string jsonString = JsonConvert.SerializeObject(listeGvg);

                HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, MEDIA_TYPE);

                var retour = await http.PostAsync($"{Outil.urlApi}/{ApiRacine.GVG}/ajouter", httpContent);

                if (retour.IsSuccessStatusCode)
                    msg = listeGvg.Count > 1 ? "Les nouvelles dates ont été programmées" : "La nouvelle date a été programmée";
                else
                    msg = "Une erreur a eu lieu";
            }
            else
            {
                msg = "Toutes les dates ont déjà été programmées";
            }

            await Context.Channel.SendMessageAsync(msg);
        }

        [Command("pingerNonInscrit")]
        public async Task PingerNonInscritProchaineGvG()
        {
            Outil outil = new();

            bool estAdmin = await outil.EstAdmin(Context.User.Id.ToString());

            if (!estAdmin)
            {
                await Context.Channel.SendMessageAsync("Tu n'as pas l'autorisation pour accéder à cette commande");
                return;
            }

            var retour = await http.GetStringAsync($"{Outil.urlApi}/{ApiRacine.GVG}/pingerNonIncritProchaineGvg");

            retour = retour.Replace('"', ' ').TrimStart();

            await Context.Channel.SendMessageAsync(retour);
        }

        [Command("supprimerGvG")]
        public async Task SupprimerGvG([Remainder] string _dateString = "")
        {
            Outil outil = new();

            if (!await outil.EstAdmin(Context.User.Id.ToString()))
            {
                await Context.Channel.SendMessageAsync("Tu n'as pas le droit");
                return;
            }

            string msg = "";

            // supp la prochaine GvG
            if (string.IsNullOrEmpty(_dateString))
            {
                string jsonString = JsonConvert.SerializeObject(new { Date = $"{_dateString}/{DateTime.Now.Year}" });
                HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, MEDIA_TYPE);

                string retour = await http.PostAsync($"{Outil.urlApi}/{ApiRacine.GVG}/supprimerViaDiscord", httpContent).Result.Content.ReadAsStringAsync();

                msg = retour;
            }
            else
            {
                bool estBloquer = false;
                List<string> listeDateString = new(_dateString.Trim().Split(','));

                foreach (var element in listeDateString)
                {
                    _dateString = Regex.Replace(_dateString, @"\s+", string.Empty);

                    if (!Regex.Match(_dateString, patternDate, RegexOptions.None).Success)
                    {
                        msg = $"Erreur: la date: {element} doit être au format JJ/MM";
                        estBloquer = true;
                        break;
                    }

                    if (!await outil.GvgExiste(element))
                    {
                        await Context.Channel.SendMessageAsync($"La GvG: {_dateString} n'existe pas");
                        return;
                    }

                    string jsonString = JsonConvert.SerializeObject(new { Date = _dateString });
                    HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, MEDIA_TYPE);

                    await http.PostAsync($"{Outil.urlApi}/{ApiRacine.GVG}/supprimerViaDiscord", httpContent);
                }

                if (!estBloquer)
                    msg = listeDateString.Count() > 1 ? "Les GvGs ont été supprimées" : "La GvG a été supprimée";
            }

            await Context.Channel.SendMessageAsync(msg);
        }
        #endregion

        #region commande public GvG

        [Command("listerGvG")]
        public async Task Lister()
        {
            string listeString = await http.GetStringAsync($"{Outil.urlApi}/{ApiRacine.GVG}/listerViaDiscord/{Context.User.Id}");

            if (listeString != "[]")
            {
                List<Gvg> listeGvg = JsonConvert.DeserializeObject<List<Gvg>>(listeString)!;

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
                await Context.Channel.SendMessageAsync("Ancunes GvGs programmées");
            }
        }

        // PAS OK
        [Command("maCompo")]
        public async Task CompoJoueur(string _dateString = "")
        {
            if (!string.IsNullOrEmpty(_dateString) && !Regex.Match(_dateString, patternDate, RegexOptions.None).Success)
            {
                await Context.Channel.SendMessageAsync($"{Context.User.Mention} Erreur: la date doit être au format JJ/MM");
                return;
            }

            string retour = await http.GetStringAsync($"{Outil.urlApi}/{ApiRacine.GVG}/listerMesUnitesViaDiscord/{_dateString}/{Context.User.Id}");

            Console.WriteLine("salut");
            if(retour != "[]")
            {
                List<Compo> listeUnite = JsonConvert.DeserializeObject<List<Compo>>(retour)!;

                EmbedBuilder embedBuilder = new()
                {
                    Color = Color.DarkPurple
                };

                string listeUniteString = "";

                foreach (Compo element in listeUnite)
                {
                    listeUniteString += $"{element.Nom} ({element.Influance})," + Environment.NewLine; 
                }

                embedBuilder.AddField("Liste des unités à prendre", listeUniteString);

                await Context.Channel.SendMessageAsync(null, false, embedBuilder.Build());
            }
            else
            {
                await Context.Channel.SendMessageAsync(retour);
            }
        }

        [Command("participer")]
        public async Task Participer(string _dateString = "")
        {
            if (!string.IsNullOrEmpty(_dateString) && !Regex.Match(_dateString, patternDate, RegexOptions.None).Success)
                await Context.Channel.SendMessageAsync($"{Context.User.Mention} Erreur: la date doit être au format JJ/MM");
            else
            {
                string jsonString = JsonConvert.SerializeObject(new { IdDiscord = Context.User.Id.ToString(), Date = _dateString + $"/{DateTime.Now.Year}" });
                HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, MEDIA_TYPE);

                string retour = await http.PostAsync($"{Outil.urlApi}/{ApiRacine.GVG}/participerViaDiscord", httpContent).Result.Content.ReadAsStringAsync();

                await Context.Channel.SendMessageAsync($"{Context.User.Mention} {retour}");
            }
        }

        [Command("absent")]
        public async Task Absent(string _dateString = "")
        {
            if (!string.IsNullOrEmpty(_dateString) && !Regex.Match(_dateString, patternDate, RegexOptions.None).Success)
                await Context.Channel.SendMessageAsync($"{Context.User.Mention} Erreur: la date doit être au format JJ/MM");
            else
            {
                string jsonString = JsonConvert.SerializeObject(new { IdDiscord = Context.User.Id.ToString(), Date = _dateString + $"/{DateTime.Now.Year}" });
                HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, MEDIA_TYPE);

                string retour = await http.PostAsync($"{Outil.urlApi}/{ApiRacine.GVG}/absentViaDiscord", httpContent).Result.Content.ReadAsStringAsync();

                await Context.Channel.SendMessageAsync($"{Context.User.Mention} {retour}");
            }
        }

        [Command("SupprimeMoi")]
        public async Task SupprimerUtilisateur()
        {
            string msg;

            string retour = await http.GetAsync($"{Outil.urlApi}/{ApiRacine.COMPTE}/supprimerViaDiscord/{Context.User.Id}").Result.Content.ReadAsStringAsync();

            bool estSupprimer = bool.Parse(retour);

            if (estSupprimer)
            {
                msg = $"Tu as été supprimé de ma base de donnée: {Context.Message.Author.Mention} bye {new Emoji("👋")} !";
            }
            else
            {
                msg = $"Je ne te trouve pas {Context.Message.Author.Mention} ton id discord n'est pas connu";
            }

            await Context.Channel.SendMessageAsync(msg);
        }
        #endregion

        #region autre
        [Command("site")]
        public async Task OuvrirSite()
        {
            await Context.Channel.SendMessageAsync("liens du site: https://conqueror-blade-gvg.jetonpeche.fr/");
        }

        [Command("strat")]
        public async Task OuvrirSiteStrat()
        {
            await Context.Channel.SendMessageAsync("lien de stratSketch https://stratsketch.com/");
        }

        [Command("aled")]
        public async Task ListerCmd()
        {
            Outil outil = new();
            bool estAdmin = await outil.EstAdmin(Context.User.Id.ToString());

            EmbedBuilder embedBuilder = new()
            {
                Title = "Liste des commandes",
                Color = Color.DarkPurple
            };

            embedBuilder.AddField("!ping", "Ping l'utilisateur");

            if (estAdmin)
            {
                embedBuilder.AddField("!pingerNonInscrit", "Ping les non inscrits à la prochaine GvG si il y en a une");
                embedBuilder.AddField("!ajouterGvG <JJ/MM> ou <JJ/MM, JJ/MM...>", "Ajout d'une ou des nouvelles dates de GvG");
                embedBuilder.AddField("!supprimerGvG <rien> ou <JJ/MM> ou <JJ/MM, JJ/MM...>", "Supprime la prochaine GvG ou la / les GvG(s) choisie(s)");
            }

            embedBuilder.AddField("!listerGvG", "Liste les GvGs programmées");
            embedBuilder.AddField("!participer <rien> ou <JJ/MM>", "Inscrit l'utilisateur pour la GvG choisie ou à la prochaine GvG");
            embedBuilder.AddField("!absent <rien> ou <JJ/MM>", "Désinscrit l'utilisateur de la prochaine GvG ou la date choisie");
            embedBuilder.AddField("!SupprimeMoi", "Supprime l'utilisateur de la base de donnée");

            embedBuilder.AddField("!site", "Affiche l'url du site");
            embedBuilder.AddField("!strat", "Affiche l'url de stratSketch");
            embedBuilder.AddField("!aled", "Liste des commandes disponibles");

            await Context.Channel.SendMessageAsync(null, false, embedBuilder.Build());
        }
        #endregion

        // [Remainder] => parametre qui accepte les espaces (exemple: salut sa va)
        // plusieur arguments => plusieur parametre de fonction
    }
}
