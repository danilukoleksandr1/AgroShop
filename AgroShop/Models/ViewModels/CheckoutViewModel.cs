using AgroShop.Web.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace AgroShop.Web.ViewModels
{
    public class CheckoutViewModel
    {
        // ===== ПІБ =====
        [Required(ErrorMessage = "Введіть ім’я")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Введіть прізвище")]
        public string LastName { get; set; } = null!;

        public string? MiddleName { get; set; }

        // ===== КОНТАКТИ =====
        [Required(ErrorMessage = "Введіть номер телефону")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Введіть електронну пошту")]
        [EmailAddress(ErrorMessage = "Невірний формат електронної пошти")]
        public string Email { get; set; } = null!;

        // ===== АДРЕСА =====
        [Required(ErrorMessage = "Вкажіть місто")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Вкажіть вулицю")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "Вкажіть номер будинку")]
        public string House { get; set; } = null!;

        public string? Apartment { get; set; }

        // ===== ДОСТАВКА =====
        [Range(1, int.MaxValue, ErrorMessage = "Оберіть спосіб доставки")]
        public int ShippingMethodID { get; set; }

        [Required(ErrorMessage = "Вкажіть деталі доставки")]
        public string ShippingDetails { get; set; } = null!;

        // ===== ОПЛАТА =====
        [Range(1, int.MaxValue, ErrorMessage = "Оберіть спосіб оплати")]
        public int PaymentMethodID { get; set; }

        // ===== SELECT =====
        [ValidateNever]
        public List<PaymentMethod> PaymentMethods { get; set; } = new();

        [ValidateNever]
        public List<ShippingMethod> ShippingMethods { get; set; } = new();
    }
}
