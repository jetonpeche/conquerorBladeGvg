namespace back.ModelExport
{
    public class CompteExport
    {
        public int Id { get; set; }
        public string Pseudo { get; set; } = null!;
        public string? IdDiscord { get; set; }
        public int EstAdmin { get; set; }
        public int Influance { get; set; }
        public int? EstPremiereConnexion { get; set; }
        public int IdClasseHeros { get; set; }
        public string NomClasseHeros { get; set; } = null!;
        public string NomImgClasse { get; set; } = null!;

        public bool? ParticipeProchaineGvg { get; set; }
        public List<int> ListeIdGvgParticipe { get; set; } = null!;
    }
}
