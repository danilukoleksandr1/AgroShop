namespace AgroShop.Web.Models
{
    public class CartItem
    {
        public int ProductID { get; set; }
        public string? Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
    }
}
