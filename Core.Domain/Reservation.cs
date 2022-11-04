using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Reservation : EntityBase
    {
        public bool IsPickedUp { get; set; }
        public Student? Student { get; set; }
        public int? StudentId { get; set; }
        public Mealbox? Mealbox { get; set; }
        public int? MealboxId { get; set; }
    }
}
