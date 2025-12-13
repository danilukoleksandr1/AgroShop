using System.Net;

namespace AgroShop.Web.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string? Username { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? Phone { get; set; }

        public int? RoleID { get; set; }
        public Role? Role { get; set; }

        public ICollection<Address>? Addresses { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
