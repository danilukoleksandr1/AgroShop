using System.ComponentModel.DataAnnotations;

namespace AgroShop.Web.ViewModels
{
    public class AddReviewViewModel
    {
        public int ProductID { get; set; }

        [Range(1, 5, ErrorMessage = "Оцінка від 1 до 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Введіть коментар")]
        public string Comment { get; set; } = null!;
    }
}

