namespace botDiscord.classe
{
    internal class Outil
    {
        public async Task<bool> EstAdmin(string _idDiscord, string _urlApi)
        {
            string estAdminString = await BasicCommande.http.GetStringAsync($"{_urlApi}/compte/estAdmin/{_idDiscord}");
            bool estAdmin = bool.Parse(estAdminString);

            return estAdmin;
        }
    }
}
