using AgroShop.Web.Data;
using AgroShop.Web.Models;
using AgroShop.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroShop.Web.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly AgroShopContext _context;

        public PaymentController(AgroShopContext context, CartService cartService)
            : base(cartService)
        {
            _context = context;
        }

        // GET: /Payment/Pay/5
        public async Task<IActionResult> Pay(int paymentId)
        {
            var payment = await _context.OrderPayments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.OrderPaymentID == paymentId);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        // POST: /Payment/Confirm
        [HttpPost]
        public async Task<IActionResult> Confirm(int paymentId)
        {
            var payment = await _context.OrderPayments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.OrderPaymentID == paymentId);

            if (payment == null)
                return NotFound();

            payment.Status = "Paid";
            payment.PayDate = DateTime.Now;

            // OPTIONAL: міняємо статус замовлення
            payment.Order.StatusID = 2; // наприклад "Оплачено"

            await _context.SaveChangesAsync();

            return RedirectToAction("Success", "Cart");
        }
    }
}
