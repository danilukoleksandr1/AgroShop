namespace AgroShop.Web.Models
{
    public class ShippingMethod
    {
        public int ShippingMethodID { get; set; }
        public string Name { get; set; } = null!;
        public decimal Cost { get; set; }
    }
}
