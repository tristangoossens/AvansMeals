using Core.Domain;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;

namespace UI.AvansMeals.Common
{
    public static class UserExtensions
    {
        public static bool IsLoggedIn(this IIdentity user)
        {
            return user.IsAuthenticated;
        }

        public static bool IsEmployee(this IIdentity user)
        {
            return ((ClaimsIdentity)user).Claims.Any(c => c.Type == "KantineMedewerker");
        }        
        public static bool IsStudent(this IIdentity user)
        {
            return ((ClaimsIdentity)user).Claims.Any(c => c.Type == "Student");
        }
    }
}
