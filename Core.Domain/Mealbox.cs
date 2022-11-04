using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Mealbox : EntityBase
    {
        public string Name { get; set; }
        public DateTime PickupFrom { get; set; }
        public DateTime PickupUntil { get; set; }
        public decimal Price { get; set; }

        public Category? Category { get; set; }

        public int CategoryId { get; set; }

        public Canteen? Canteen { get; set; }

        public int CanteenId { get; set; }

        public Reservation? Reservation { get; set; }
    }
}
