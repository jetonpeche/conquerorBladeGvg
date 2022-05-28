using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class GvgUniteCompte
    {
        public int IdGvg { get; set; }
        public int IdCompte { get; set; }
        public int IdUnite { get; set; }

        public virtual Compte IdCompteNavigation { get; set; } = null!;
        public virtual Gvg IdGvgNavigation { get; set; } = null!;
        public virtual Unite IdUniteNavigation { get; set; } = null!;
    }
}
