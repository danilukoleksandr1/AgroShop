using AgroShop.Web.Models;

namespace AgroShop.Web.ViewModels
{
    public class ProfileViewModel
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<Order> Orders { get; set; } = new();
    }
}
