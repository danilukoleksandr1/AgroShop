using AgroShop.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace AgroShop.Web.ViewModels
{
    public class CheckoutViewModel
    {
        // ===== ПІБ ОТРИМУВАЧА =====
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        // ===== КОНТАКТИ =====
        [Required]
        public string Phone { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        // ===== АДРЕСА ДОСТАВКИ =====
        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string House { get; set; }

        public string Apartment { get; set; }

        // ===== СПОСОБИ =====
        [Required]
        public int PaymentMethodID { get; set; }

        [Required]
        public int ShippingMethodID { get; set; }

        // ===== ДАНІ ДЛЯ SELECT =====
        public List<PaymentMethod> PaymentMethods { get; set; }
        public List<ShippingMethod> ShippingMethods { get; set; }
    }
}
