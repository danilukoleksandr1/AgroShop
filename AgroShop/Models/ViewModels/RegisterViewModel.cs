using System.ComponentModel.DataAnnotations;

namespace AgroShop.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введіть ім'я користувача")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Введіть Email")]
        [EmailAddress(ErrorMessage = "Невірний формат Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Введіть пароль")]
        [MinLength(6, ErrorMessage = "Пароль має містити мінімум 6 символів")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Підтвердіть пароль")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
