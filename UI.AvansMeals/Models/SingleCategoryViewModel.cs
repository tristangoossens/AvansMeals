namespace UI.AvansMeals.Models
{
    public class SingleCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageBase64 { get; set; }
        public bool IsAgeBound { get; set; }
        public int ProductCount { get; set; }
        public List<SingleProductViewModel> Products { get; set; }
    }
}
