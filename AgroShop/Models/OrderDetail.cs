using System.ComponentModel.DataAnnotations;

namespace AgroShop.Web.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }
        public int ProductID { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Order Order { get; set; } = null!;
    }
}
