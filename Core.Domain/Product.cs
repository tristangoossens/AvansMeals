using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Product : EntityBase
    {
        public string Name { get; set; }
        public bool AgeBound { get; set; }
        public byte[] Image { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
    }
}
