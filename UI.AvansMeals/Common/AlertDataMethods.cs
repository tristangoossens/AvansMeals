using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace UI.AvansMeals.Common
{
    public static class TempDataExtensions
    {
        public static void SetErrorData(this ITempDataDictionary @this, string message)
        {
            @this["ERROR"] = true;
            @this["MESSAGE"] = message;
        }

        public static void SetSuccessData(this ITempDataDictionary @this, string message)
        {
            @this["SUCCESS"] = true;
            @this["MESSAGE"] = message;
        }
    }
}
