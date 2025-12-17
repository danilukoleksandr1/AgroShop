using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgroShop.Web.Models
{
    [Table("OrderStatus")] // 👈 ВАЖЛИВО
    public class OrderStatus
    {
        [Key]
        public int StatusID { get; set; }
        public string StatusName { get; set; } = null!;
    }
}
