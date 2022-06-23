namespace botDiscord.classe
{
    internal class Outil
    {
        //static string urlApi { get; } = "http://localhost:5019";
        public static string urlApi { get; } = "https://cb-gvg-api.jetonpeche.fr";

        public async Task<bool> EstAdmin(string _idDiscord)
        {
            string estAdminString = await BasicCommande.http.GetStringAsync($"{urlApi}/{ApiRacine.COMPTE}/estAdmin/{_idDiscord}");
            bool estAdmin = bool.Parse(estAdminString);

            return estAdmin;
        }
    }
}
