using AgroShop.Web.Data;
using AgroShop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AgroShop.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminProductsController : Controller
    {
        private readonly AgroShopContext _context;

        public AdminProductsController(AgroShopContext context)
        {
            _context = context;
        }

        // ===== Список товарів =====
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .Include(p => p.Category)
                .ToListAsync());
        }

        // ===== Додати товар (форма) =====
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // ===== Додати товар (збереження) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, string? NewCategoryName)
        {
            ViewBag.Categories = _context.Categories.ToList();

            if (!ModelState.IsValid)
                return View(product);

            if (!string.IsNullOrWhiteSpace(NewCategoryName))
            {
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == NewCategoryName.Trim().ToLower());

                if (existingCategory != null)
                {
                    ViewBag.CategoryError = "Категорія з такою назвою вже існує.";
                    product.CategoryID = existingCategory.CategoryID;
                    return View(product);
                }

                var category = new Category { Name = NewCategoryName.Trim() };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                product.CategoryID = category.CategoryID;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // ===== Редагування (форма) =====
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name", product.CategoryID);
            return View(product);
        }

        // ===== Редагування (збереження) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "Name", product.CategoryID);
                return View(product);
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // ===== Сховати/Показати продукт =====
        public async Task<IActionResult> ToggleActive(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.IsActive = !product.IsActive;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
