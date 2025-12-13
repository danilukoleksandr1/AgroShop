namespace AgroShop.Web.Models
{
    public class Address
    {
        public int AddressID { get; set; }
        public int? UserID { get; set; }
        public User? User { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string House { get; set; } = null!;
        public string? Apartment { get; set; }
        public string? PostalCode { get; set; }
        public string? Comment { get; set; }
    }
}
