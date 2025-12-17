using System.ComponentModel.DataAnnotations;

namespace AgroShop.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введіть Email")]
        [EmailAddress(ErrorMessage = "Невірний формат Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Введіть пароль")]
        public string Password { get; set; } = null!;
    }
}
