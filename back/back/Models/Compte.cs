using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class Compte
    {
        public Compte()
        {
            GvgComptes = new HashSet<GvgCompte>();
            GvgUniteComptes = new HashSet<GvgUniteCompte>();
            UniteComptes = new HashSet<UniteCompte>();
        }

        public int Id { get; set; }
        public int IdClasseHeros { get; set; }
        public string? IdDiscord { get; set; }
        public string Pseudo { get; set; } = null!;
        public int Influance { get; set; }
        public int? EstPremiereConnexion { get; set; }
        public int EstAdmin { get; set; }

        public virtual ClasseHero IdClasseHerosNavigation { get; set; } = null!;
        public virtual ICollection<GvgCompte> GvgComptes { get; set; }
        public virtual ICollection<GvgUniteCompte> GvgUniteComptes { get; set; }
        public virtual ICollection<UniteCompte> UniteComptes { get; set; }
    }
}
