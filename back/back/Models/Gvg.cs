using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class Gvg
    {
        public Gvg()
        {
            GvgComptes = new HashSet<GvgCompte>();
            GvgUniteComptes = new HashSet<GvgUniteCompte>();
        }

        public int Id { get; set; }
        public DateTime DateProgrammer { get; set; }

        public virtual ICollection<GvgCompte> GvgComptes { get; set; }
        public virtual ICollection<GvgUniteCompte> GvgUniteComptes { get; set; }
    }
}
