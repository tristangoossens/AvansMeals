using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace UI.AvansMeals.Models
{
    public enum FilterMenuSortType
    {
        [Display(Name = "Datum oplopend")]
        DATE_ASC,

        [Display(Name = "Datum aflopend")]
        DATE_DESC,

        [Display(Name = "Prijs laag - hoog")]
        PRICE_ASC,

        [Display(Name = "Prijs hoog - laag")]
        PRICE_DESC,
    }
}
