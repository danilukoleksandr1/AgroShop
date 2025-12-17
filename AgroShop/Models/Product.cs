namespace AgroShop.Web.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int? CategoryID { get; set; }
        public Category? Category { get; set; }
        public string? Manufacturer { get; set; }
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true; // додати після ImageUrl

        public ICollection<Review>? Reviews { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
