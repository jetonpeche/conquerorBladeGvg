using System;
using System.Collections.Generic;

namespace back.Models
{
    public partial class TypeUnite
    {
        public TypeUnite()
        {
            Unites = new HashSet<Unite>();
        }

        public int Id { get; set; }
        public string Nom { get; set; } = null!;

        public virtual ICollection<Unite> Unites { get; set; }
    }
}
