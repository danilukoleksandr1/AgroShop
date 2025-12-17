using System.ComponentModel.DataAnnotations;

namespace AgroShop.Web.Models
{
    public class OrderShipping
    {
        [Key]
        public int OrderShippingID { get; set; }

        public int OrderID { get; set; }
        public int ShippingMethodID { get; set; }

        public string ShippingDetails { get; set; } = null!;
        public decimal Cost { get; set; }

        public Order Order { get; set; } = null!;
    }
}
