using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgroShop.Web.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        // ===== USER =====
        public int? UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public User? User { get; set; }

        // ===== STATUS =====
        public int StatusID { get; set; }

        [ForeignKey(nameof(StatusID))]
        public OrderStatus Status { get; set; } = null!;

        // ===== DATE / TOTAL =====
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        // ===== 1 : 1 =====
        public OrderContact Contact { get; set; } = null!;
        public OrderAddress Address { get; set; } = null!;
        public OrderShipping Shipping { get; set; } = null!;

        // ===== 1 : M =====
        public List<OrderDetail> Details { get; set; } = new();
        public List<OrderPayment> Payments { get; set; } = new();
    }
}
