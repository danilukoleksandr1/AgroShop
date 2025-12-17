using AgroShop.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgroShop.Web.ViewModels
{
    public class AdminProductViewModel
    {
        public Product Product { get; set; } = new Product();

        public List<SelectListItem> Categories { get; set; } = new();

        // для створення нової категорії
        public string? NewCategoryName { get; set; }
        public string? NewCategoryDescription { get; set; }
    }
}
