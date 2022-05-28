using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class Groupe
    {
        public Groupe()
        {
            GvgComptes = new HashSet<GvgCompte>();
        }

        public int Id { get; set; }
        public string Nom { get; set; } = null!;

        public virtual ICollection<GvgCompte> GvgComptes { get; set; }
    }
}
