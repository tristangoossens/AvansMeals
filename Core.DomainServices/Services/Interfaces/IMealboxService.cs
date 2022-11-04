using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services.Interfaces
{
    public interface IMealboxService
    {
        Task<Mealbox> CreateNewMealbox(
            string name,
            DateTime pickupFrom,
            DateTime pickupUntil,
            decimal price,
            int categoryId,
            int canteenId
        );

        // Edit en delete 
        Task<Mealbox> CreateModifiedMealbox(
            int id,
            string name,
            DateTime pickupFrom,
            DateTime pickupUntil,
            decimal price,
            int categoryId,
            int canteenId
        );

        Task<bool> IsAgeBound(int mealBoxId);
        Task<bool> IsReserved(int mealBoxId);
    }
}
