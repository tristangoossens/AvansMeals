using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Employee : EntityBase
    {
        public string Email { get; set; }
        public int EmployeeNr { get; set; }
        public string Name { get; set; }
        public Canteen? Canteen { get; set; }

        public int? CanteenId { get; set; }
    }
}
