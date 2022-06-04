namespace botDiscord.models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string IdDiscord { get; set; } = null!;
        public string NomDiscord { get; set; } = null!;
    }
}
