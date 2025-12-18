using AgroShop.Web.Data;
using AgroShop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroShop.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCategoriesController : Controller
    {
        private readonly AgroShopContext _context;

        public AdminCategoriesController(AgroShopContext context)
        {
            _context = context;
        }

        // Список категорій
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // Додати категорію (форма) 
        public IActionResult Create()
        {
            return View();
        }

        // Додати категорію (збереження)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            // Перевірка на існування категорії
            var existing = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == category.Name.Trim().ToLower());

            if (existing != null)
            {
                ViewBag.ErrorMessage = "Категорія з такою назвою вже існує.";
                return View(category);
            }

            category.Name = category.Name.Trim();
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Видалити категорію 
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
                return NotFound();

            if (category.Products != null && category.Products.Any())
            {
                TempData["ErrorMessage"] = "Цю категорію не можна видалити, бо у ній є товари.";
                return RedirectToAction("Index");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        // Сховати/Показати категорію
        public async Task<IActionResult> ToggleActive(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            category.IsActive = !category.IsActive;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
