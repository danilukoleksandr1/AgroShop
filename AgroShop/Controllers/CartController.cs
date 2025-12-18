using AgroShop.Web.Data;
using AgroShop.Web.Models;
using AgroShop.Web.Services;
using AgroShop.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AgroShop.Web.Controllers
{
    public class CartController : BaseController
    {
        private readonly AgroShopContext _context;

        public CartController(AgroShopContext context, CartService cartService)
            : base(cartService)
        {
            _context = context;
        }

        //  CART SESSION 
        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString("Cart");
            return json == null
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(json)!;
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart",
                JsonSerializer.Serialize(cart));
        }

        //ADD додоти в кошик
        public async Task<IActionResult> Add(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            //  якщо немає в наявності
            if (product.Stock <= 0)
                return RedirectToAction("Details", "Products", new { id });

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
                //  не більше ніж є на складі
                if (item.Quantity < product.Stock)
                    item.Quantity++;
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // перегляд кошика
        public IActionResult Index()
        {
            var cart = GetCart();

            foreach (var item in cart)
            {
                var product = _context.Products
                    .FirstOrDefault(p => p.ProductID == item.ProductID);

                item.AvailableStock = product?.Stock ?? 0;
            }

            return View(cart);
        }


        // зміна кількості товару в кошику
        [HttpPost]
        public IActionResult Update(int id, int qty)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductID == id);
            if (item == null) return RedirectToAction("Index");

            var product = _context.Products.FirstOrDefault(p => p.ProductID == id);
            if (product == null) return RedirectToAction("Index");

            if (qty > product.Stock)
            {
                TempData["Error"] = $"В наявності лише {product.Stock} шт.";
                qty = product.Stock;
            }

            if (qty <= 0)
            {
                cart.Remove(item);
            }
            else
            {
                item.Quantity = qty;
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }


        // видалити товар з кошика
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

        // CHECKOUT GET 
        public IActionResult Checkout()
        {
            var vm = new CheckoutViewModel
            {
                PaymentMethods = _context.PaymentMethods.ToList(),
                ShippingMethods = _context.ShippingMethods.ToList()
            };

            return View(vm);
        }

        //  CHECKOUT POST оформлення замовлення
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.PaymentMethods = await _context.PaymentMethods.ToListAsync();
                vm.ShippingMethods = await _context.ShippingMethods.ToListAsync();
                return View(vm);
            }

            var cart = GetCart();
            if (!cart.Any())
                return RedirectToAction("Index");

            // перевірка складу перед оформленням
            foreach (var item in cart)
            {
                var product = await _context.Products.FindAsync(item.ProductID);
                if (product == null || product.Stock < item.Quantity)
                {
                    ModelState.AddModelError("", $"Недостатньо товару: {item.Name}");
                    vm.PaymentMethods = await _context.PaymentMethods.ToListAsync();
                    vm.ShippingMethods = await _context.ShippingMethods.ToListAsync();
                    return View(vm);
                }
            }

            decimal total = cart.Sum(x => x.UnitPrice * x.Quantity);

            int? userId = null;

            if (User.Identity!.IsAuthenticated)
            {
                userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            }

            var order = new Order
            {
                UserID = userId,
                StatusID = 1,
                OrderDate = DateTime.Now,
                TotalAmount = total
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _context.OrderContacts.Add(new OrderContact
            {
                OrderID = order.OrderID,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                MiddleName = vm.MiddleName,
                Phone = vm.Phone,
                Email = vm.Email
            });

            _context.OrderAddresses.Add(new OrderAddress
            {
                OrderID = order.OrderID,
                City = vm.City,
                Street = vm.Street,
                House = vm.House,
                Apartment = vm.Apartment
            });

            _context.OrderShipping.Add(new OrderShipping
            {
                OrderID = order.OrderID,
                ShippingMethodID = vm.ShippingMethodID,
                ShippingDetails = vm.ShippingDetails,
                Cost = 0
            });

            //  деталі +  мінус зі складу
            foreach (var item in cart)
            {
                var product = await _context.Products.FindAsync(item.ProductID);

                product!.Stock -= item.Quantity;

                _context.OrderDetails.Add(new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            await _context.SaveChangesAsync();

            // платіж
            var payment = new OrderPayment
            {
                OrderID = order.OrderID,
                PaymentMethodID = vm.PaymentMethodID,
                Amount = total,
                Status = vm.PaymentMethodID == 2 ? "Pending" : "Paid",
                PayDate = vm.PaymentMethodID == 2 ? null : DateTime.Now
            };
            _context.OrderPayments.Add(payment);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");

            if (vm.PaymentMethodID == 2)
                return RedirectToAction("Pay", "Payment", new { paymentId = payment.OrderPaymentID });

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
