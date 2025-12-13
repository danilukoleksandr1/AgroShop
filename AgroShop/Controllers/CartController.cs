using AgroShop.Web.Data;
using AgroShop.Web.Models;
using AgroShop.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AgroShop.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly AgroShopContext _context;

        public CartController(AgroShopContext context)
        {
            _context = context;
        }

        // ============================
        // SESSION: КОШИК
        // ============================
        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString("Cart");
            return json == null
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(json);
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart",
                JsonSerializer.Serialize(cart));
        }

        // ============================
        // ДОДАТИ В КОШИК
        // ============================
        public async Task<IActionResult> Add(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductID == id);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    UnitPrice = product.Price,
                    Quantity = 1
                });
            }
            else
            {
                item.Quantity++;
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // ============================
        // ПЕРЕГЛЯД КОШИКА
        // ============================
        public IActionResult Index()
        {
            return View(GetCart());
        }

        // ============================
        // ОНОВЛЕННЯ КІЛЬКОСТІ
        // ============================
        [HttpPost]
        public IActionResult Update(int id, int qty)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductID == id);

            if (item != null && qty > 0)
            {
                item.Quantity = qty;
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        // ============================
        // ВИДАЛЕННЯ
        // ============================
        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductID == id);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        // ============================
        // CHECKOUT (GET)
        // ============================
        public async Task<IActionResult> Checkout()
        {
            var vm = new CheckoutViewModel
            {
                PaymentMethods = await _context.PaymentMethods.ToListAsync(),
                ShippingMethods = await _context.ShippingMethods.ToListAsync()
            };

            return View(vm);
        }

        // ============================
        // CHECKOUT (POST)
        // ============================
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var cart = GetCart();
            if (!cart.Any())
                return RedirectToAction("Index");

            // 1️⃣ СТВОРЮЄМО АДРЕСУ
            var address = new Address
            {
                UserID = 1, // тимчасово (без авторизації)
                City = vm.City,
                Street = vm.Street,
                House = vm.House,
                Apartment = vm.Apartment
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            // 2️⃣ СТВОРЮЄМО ЗАМОВЛЕННЯ
            var order = new Order
            {
                UserID = 1,
                StatusID = 1,
                AddressID = address.AddressID,
                PaymentMethodID = vm.PaymentMethodID,
                ShippingMethodID = vm.ShippingMethodID,

                DeliveryLastName = vm.LastName,
                DeliveryFirstName = vm.FirstName,
                DeliveryMiddleName = vm.MiddleName,

                Phone = vm.Phone,
                Email = vm.Email,

                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(x => x.UnitPrice * x.Quantity)
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 3️⃣ (за бажанням) OrderDetails
            foreach (var item in cart)
            {
                _context.OrderDetails.Add(new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Success");
        }

        // ============================
        // УСПІШНО
        // ============================
        public IActionResult Success()
        {
            return View();
        }
    }
}
