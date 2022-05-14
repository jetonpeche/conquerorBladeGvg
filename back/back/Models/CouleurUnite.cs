using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class CouleurUnite
    {
        public CouleurUnite()
        {
            Unites = new HashSet<Unite>();
        }

        public int Id { get; set; }
        public string Nom { get; set; } = null!;

        public virtual ICollection<Unite> Unites { get; set; }
    }
}
