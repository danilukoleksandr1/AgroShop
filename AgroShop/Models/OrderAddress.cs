using System.ComponentModel.DataAnnotations;

namespace AgroShop.Web.Models
{
    public class OrderAddress
    {
        [Key]
        public int OrderAddressID { get; set; }

        public int OrderID { get; set; }

        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string House { get; set; } = null!;
        public string? Apartment { get; set; }

        public Order Order { get; set; } = null!;
    }
}
