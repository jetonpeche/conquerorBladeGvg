using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class ClasseHero
    {
        public ClasseHero()
        {
            Comptes = new HashSet<Compte>();
        }

        public int Id { get; set; }
        public string Nom { get; set; } = null!;
        public string IconClasse { get; set; } = null!;
        public string NomImg { get; set; } = null!;

        public virtual ICollection<Compte> Comptes { get; set; }
    }
}
