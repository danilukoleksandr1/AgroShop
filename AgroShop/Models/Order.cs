namespace AgroShop.Web.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int? UserID { get; set; }
        public User? User { get; set; }
        public int? StatusID { get; set; }
        public OrderStatus? Status { get; set; }
        public int? AddressID { get; set; }
        public Address? Address { get; set; }
        public int? PaymentMethodID { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public int? ShippingMethodID { get; set; }
        public ShippingMethod? ShippingMethod { get; set; }

        public string DeliveryLastName { get; set; } = null!;
        public string DeliveryFirstName { get; set; } = null!;
        public string? DeliveryMiddleName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

    }
}
