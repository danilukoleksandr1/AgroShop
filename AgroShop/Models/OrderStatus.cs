using System.ComponentModel.DataAnnotations;

namespace AgroShop.Web.Models
{
    public class OrderStatus
    {
        [Key]
        public int StatusID { get; set; }

        public string StatusName { get; set; } = null!;

        public ICollection<Order>? Orders { get; set; }
    }
}

