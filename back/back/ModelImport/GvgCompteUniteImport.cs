namespace back.ModelImport
{
    public class GvgCompteUniteImport
    {
        public IdCompteUniteGvg[] ListeUniteAjouter { get; set; } = null!;
        public IdCompteUniteGvg[] ListeUniteAsupprimer { get; set; } = null!;
    }

    public class IdCompteUniteGvg
    {
        public int IdGvg { get; set; }
        public int IdCompte { get; set; }
        public int IdUnite { get; set; }
    }
}
