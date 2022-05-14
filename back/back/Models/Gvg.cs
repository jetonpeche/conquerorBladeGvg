using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class Gvg
    {
        public Gvg()
        {
            GvgUniteComptes = new HashSet<GvgUniteCompte>();
            IdComptes = new HashSet<Compte>();
        }

        public int Id { get; set; }
        public DateTime DateProgrammer { get; set; }

        public virtual ICollection<GvgUniteCompte> GvgUniteComptes { get; set; }

        public virtual ICollection<Compte> IdComptes { get; set; }
    }
}
