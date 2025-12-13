using AgroShop.Web.Data;
using AgroShop.Web.Models;
using AgroShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AgroShopContext _context;

        public ProductsController(AgroShopContext context)
        {
            _context = context;
        }

        // ------------------ CATALOG ------------------
        public async Task<IActionResult> Index(string? search, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var vm = new ProductFilterViewModel
            {
                Categories = await _context.Categories.ToListAsync()
            };

            IQueryable<Product> query = _context.Products.Include(p => p.Category);

            // SEARCH
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.Description.Contains(search));
            }

            // CATEGORY FILTER
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryID == categoryId.Value);
                vm.CategoryID = categoryId;
            }

            // PRICE FILTER
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
                vm.MinPrice = minPrice;
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
                vm.MaxPrice = maxPrice;
            }

            vm.Search = search;
            vm.Products = await query.ToListAsync();

            return View(vm);
        }

        // ------------------ PRODUCT DETAILS ------------------
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}
