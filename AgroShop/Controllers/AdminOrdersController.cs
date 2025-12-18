using AgroShop.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroShop.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminOrdersController : Controller
    {
        private readonly AgroShopContext _context;

        public AdminOrdersController(AgroShopContext context)
        {
            _context = context;
        }

        // список замовлень
        public async Task<IActionResult> Index()
        {
            ViewBag.Statuses = await _context.OrderStatuses.ToListAsync();

            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Status)
                .ToListAsync();

            return View(orders);
        }

        // зміна статусу (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int orderId, int statusId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return NotFound();

            var statusExists = await _context.OrderStatuses
                .AnyAsync(s => s.StatusID == statusId);

            if (!statusExists)
                return BadRequest();

            order.StatusID = statusId;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
