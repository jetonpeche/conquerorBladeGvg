using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class Compte
    {
        public Compte()
        {
            GvgUniteComptes = new HashSet<GvgUniteCompte>();
            UniteComptes = new HashSet<UniteCompte>();
            IdGvgs = new HashSet<Gvg>();
        }

        public int Id { get; set; }
        public int IdClasseHeros { get; set; }
        public string? IdDiscord { get; set; }
        public string? NomDiscord { get; set; }
        public string Pseudo { get; set; } = null!;
        public int Influance { get; set; }
        public int? EstPremiereConnexion { get; set; }

        public virtual ClasseHero IdClasseHerosNavigation { get; set; } = null!;
        public virtual ICollection<GvgUniteCompte> GvgUniteComptes { get; set; }
        public virtual ICollection<UniteCompte> UniteComptes { get; set; }

        public virtual ICollection<Gvg> IdGvgs { get; set; }
    }
}
