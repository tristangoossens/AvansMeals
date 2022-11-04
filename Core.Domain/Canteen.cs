using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Canteen : EntityBase
    {
        public string Name { get; set; }
        public bool WarmMeals { get; set; }

        public City City { get; set; }
        public IEnumerable<Employee>? Employees { get; set; }
        public IEnumerable<Mealbox>? Mealboxes { get; set; }
    }
}
