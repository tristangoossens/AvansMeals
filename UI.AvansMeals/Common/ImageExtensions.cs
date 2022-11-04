using Core.Domain;

namespace UI.AvansMeals.Common
{
    public static class ImageExtensions
    {
        public static string ToBase64String(this byte[] image)
        {
            return $"data:image/gif;base64,{Convert.ToBase64String(image)}";
        }
    }
}
