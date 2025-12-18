using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgroShop.Web.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        
        public int? UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public User? User { get; set; }

        public int StatusID { get; set; }

        [ForeignKey(nameof(StatusID))]
        public OrderStatus Status { get; set; } = null!;

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public OrderContact Contact { get; set; } = null!;
        public OrderAddress Address { get; set; } = null!;
        public OrderShipping Shipping { get; set; } = null!;

        public List<OrderDetail> Details { get; set; } = new();
        public List<OrderPayment> Payments { get; set; } = new();
    }
}
