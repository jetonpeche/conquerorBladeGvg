using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class GvgCompte
    {
        public int IdGvg { get; set; }
        public int IdCompte { get; set; }
        public int? IdGroupe { get; set; }

        public virtual Compte IdCompteNavigation { get; set; } = null!;
        public virtual Groupe? IdGroupeNavigation { get; set; }
        public virtual Gvg IdGvgNavigation { get; set; } = null!;
    }
}
