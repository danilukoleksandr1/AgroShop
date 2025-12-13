namespace AgroShop.Web.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
