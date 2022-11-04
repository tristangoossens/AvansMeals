using Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UI.AvansMeals.Models;

namespace UI.AvansMeals.Common
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

        public static IQueryable<Mealbox> Sort(this FilterMenuSortType enumValue, IQueryable<Mealbox> mealboxes)
        {
            switch (enumValue)
            {
                case FilterMenuSortType.DATE_DESC:
                    mealboxes = mealboxes.OrderByDescending(mb => mb.PickupFrom).AsQueryable();
                    break;
                case FilterMenuSortType.DATE_ASC:
                    mealboxes = mealboxes.OrderBy(mb => mb.PickupFrom).AsQueryable();
                    break;
                case FilterMenuSortType.PRICE_DESC:
                    mealboxes = mealboxes.OrderByDescending(mb => mb.Price).AsQueryable();
                    break;
                case FilterMenuSortType.PRICE_ASC:
                    mealboxes = mealboxes.OrderBy(mb => mb.Price).AsQueryable();
                    break;
            }

            return mealboxes;
        }
    }
}
