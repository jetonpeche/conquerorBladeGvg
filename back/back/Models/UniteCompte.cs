using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class UniteCompte
    {
        public int IdCompte { get; set; }
        public int IdUnite { get; set; }
        public int EstTemporaire { get; set; }
        public string NiveauMaitrise { get; set; } = null!;

        public virtual Compte IdCompteNavigation { get; set; } = null!;
        public virtual Unite IdUniteNavigation { get; set; } = null!;
    }
}
