using Newtonsoft.Json;
using System.Text;

namespace botDiscord.classe
{
    internal class Outil
    {
        public static string urlApi { get; } = "http://localhost:5019";
        //public static string urlApi { get; } = "https://cb-gvg-api.jetonpeche.fr";

        public async Task<bool> EstAdmin(string _idDiscord)
        {
            string estAdminString = await BasicCommande.http.GetStringAsync($"{urlApi}/{ApiRacine.COMPTE}/estAdmin/{_idDiscord}");
            bool estAdmin = bool.Parse(estAdminString);

            return estAdmin;
        }

        public async Task<bool>GvgExiste(string _dateString)
        {
            string jsonString = JsonConvert.SerializeObject(new { Date = _dateString + $"/{DateTime.Now.Year}" });
            HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, BasicCommande.MEDIA_TYPE);

            string retour = await BasicCommande.http.PostAsync($"{urlApi}/{ApiRacine.GVG}/existe", httpContent).Result.Content.ReadAsStringAsync();

            bool existe = bool.Parse(retour);

            return existe;
        }
    }
}
