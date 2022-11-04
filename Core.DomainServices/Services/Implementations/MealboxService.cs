using Core.Domain;
using Core.DomainServices.Repositories;
using Core.DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services.Implementations
{
    public class MealboxService : IMealboxService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICanteenRepository _canteenRepository;
        private readonly IMealboxRepository _mealboxRepository;


        public MealboxService(
            ICategoryRepository categoryRepository,
            ICanteenRepository canteenRepository,
            IMealboxRepository mealboxRepository
        )
        {
            _categoryRepository = categoryRepository;
            _canteenRepository = canteenRepository;
            _mealboxRepository = mealboxRepository;
        }

        public async Task<Mealbox> CreateNewMealbox(string name, DateTime pickupFrom, DateTime pickupUntil, decimal price, int categoryId, int canteenId)
        {
            if (!ValidatePickupDate(pickupFrom, pickupUntil)) throw new ArgumentException("De ingegeven datums voor het ophalen van dit pakket zijn incorrect");

            return new Mealbox
            {
                Name = name,
                PickupFrom = pickupFrom,
                PickupUntil = pickupUntil,
                Price = price,
                Category = await GetRelatedCategory(categoryId),
                Canteen = await GetRelatedCanteen(canteenId)
            };
        }

        public async Task<Mealbox> CreateModifiedMealbox(int id, string name, DateTime pickupFrom, DateTime pickupUntil, decimal price, int categoryId, int canteenId)
        {
            if (await IsReserved(id))
                throw new InvalidOperationException("Dit pakket is reeds gereserveerd door een gebruiker");

            return new Mealbox
            {
                Id = id,
                Name = name,
                PickupFrom = pickupFrom,
                PickupUntil = pickupUntil,
                Price = price,
                Category = await GetRelatedCategory(categoryId),
                Canteen = await GetRelatedCanteen(canteenId)
            };
        }

        public async Task<bool> IsAgeBound(int mealBoxId)
        {
            Mealbox mealbox = await _mealboxRepository.GetSingle(mealBoxId);
            return mealbox.Category.Products.Any(p => p.AgeBound);
        }

        public async Task<bool> IsReserved(int mealboxId)
        {
            Mealbox mealbox = await _mealboxRepository.GetSingle(mealboxId);
            return mealbox.Reservation != null;
        }

        private bool ValidatePickupDate(DateTime pickUpDateFrom, DateTime pickUpDateUntil)
        {
            // Pickup dates are not on the same day
            if (pickUpDateFrom.Date != pickUpDateUntil.Date) return false;

            // Pickup deadline is set before pickup date OR pickup date is set before today
            if (pickUpDateFrom > pickUpDateUntil || pickUpDateFrom < DateTime.Today) return false;

            // Pickup date is set after the maximum gap of two days from now
            if ((pickUpDateFrom - DateTime.Now).TotalDays <= 2) return false;


            // Pickup dates are valid
            return true;
        }

        private async Task<Category> GetRelatedCategory(int categoryId)
        {
            return await _categoryRepository.GetSingle(categoryId);
        }

        private async Task<Canteen> GetRelatedCanteen(int canteenId)
        {
            return await _canteenRepository.GetSingle(canteenId);
        }
    }
}
