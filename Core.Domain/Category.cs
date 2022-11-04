using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Category : EntityBase
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public IEnumerable<Product>? Products { get; set; }
        public IEnumerable<Mealbox>? Mealboxes { get; set; }
    }
}
