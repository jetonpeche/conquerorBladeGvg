using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class Unite
    {
        public Unite()
        {
            GvgUniteComptes = new HashSet<GvgUniteCompte>();
            UniteComptes = new HashSet<UniteCompte>();
        }

        public int Id { get; set; }
        public int IdCouleur { get; set; }
        public int IdTypeUnite { get; set; }
        public string Nom { get; set; } = null!;
        public string NomImg { get; set; } = null!;
        public int Influance { get; set; }
        public int EstMeta { get; set; }

        public virtual CouleurUnite IdCouleurNavigation { get; set; } = null!;
        public virtual TypeUnite IdTypeUniteNavigation { get; set; } = null!;
        public virtual ICollection<GvgUniteCompte> GvgUniteComptes { get; set; }
        public virtual ICollection<UniteCompte> UniteComptes { get; set; }
    }
}
