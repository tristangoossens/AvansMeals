using Core.Domain;
using System.Security.Principal;

namespace UI.AvansMeals.Models
{
    public class LoggedInUserViewModel
    {
        public bool IsLoggedIn { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsStudent { get; set; }

        // Optional attribute (based on bools)
        public IIdentity? CurrentUser { get; set; }
    }
}
