namespace AgroShop.Web.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true; 

        public ICollection<Product>? Products { get; set; }
    }
}
