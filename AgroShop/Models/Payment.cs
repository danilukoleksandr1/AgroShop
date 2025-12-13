namespace AgroShop.Web.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int? OrderID { get; set; }
        public Order? Order { get; set; }
        public int? PaymentMethodID { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime PayDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
