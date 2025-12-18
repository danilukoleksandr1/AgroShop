using AgroShop.Web.Data;
using AgroShop.Web.Models;
using AgroShop.Web.Models.ViewModels;
using AgroShop.Web.Services;
using AgroShop.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AgroShop.Web.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly AgroShopContext _context;

        public ProductsController(AgroShopContext context, CartService cartService)
            : base(cartService)
        {
            _context = context;
        }


        // каталог
        public async Task<IActionResult> Index(string? search, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var vm = new ProductFilterViewModel
            {
                Categories = await _context.Categories.Where(c => c.IsActive).ToListAsync()
            };


            // Показуємо лише активні товари та категорії
            IQueryable<Product> query = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && (p.Category == null || p.Category.IsActive));


            // пошук товару
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.Description.Contains(search));
            }

            // фільтрація за категоріями
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryID == categoryId.Value);
                vm.CategoryID = categoryId;
            }

            // фільтр за ціною
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

        // сторінка товару
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

        // додавання відгуків

        [HttpPost]
        public async Task<IActionResult> AddReview(AddReviewViewModel vm)
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return RedirectToAction("Details", new { id = vm.ProductID });

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var review = new Review
            {
                ProductID = vm.ProductID,
                UserID = userId,
                Rating = vm.Rating,
                Comment = vm.Comment,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = vm.ProductID });
        }
    }
}
