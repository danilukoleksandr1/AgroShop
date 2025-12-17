using System.ComponentModel.DataAnnotations;

namespace AgroShop.Web.Models
{
    public class OrderContact
    {
        [Key]
        public int OrderContactID { get; set; }

        public int OrderID { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }

        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;

        public Order Order { get; set; } = null!;
    }
}
