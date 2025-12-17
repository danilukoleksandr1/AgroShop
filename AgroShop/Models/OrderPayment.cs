using System.ComponentModel.DataAnnotations;

namespace AgroShop.Web.Models
{
    public class OrderPayment
    {
        [Key]
        public int OrderPaymentID { get; set; }

        public int OrderID { get; set; }
        public int PaymentMethodID { get; set; }

        public decimal Amount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime? PayDate { get; set; }

        public Order Order { get; set; } = null!;
    }
}
