namespace AgroShop.Web.Models.ViewModels
{
    public class ProductFilterViewModel
    {
        public string? Search { get; set; }
        public int? CategoryID { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public List<Category>? Categories { get; set; }
        public List<Product>? Products { get; set; }
    }
}
