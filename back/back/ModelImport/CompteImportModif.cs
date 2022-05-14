namespace back.ModelImport
{
    public class CompteImportModif: CompteImport
    {
        public int IdCompte { get; set; }
        public List<UniteNiv> ListeUniteNiv { get; set; } = null!;
    }

    public class UniteNiv
    {
        public int Id { get; set; }
        public string Niveau { get; set; }
    }
}
