using AgroShop.Web.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AgroShop.Web.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _http;

        private const string CART_KEY = "CART";

        public CartService(IHttpContextAccessor http)
        {
            _http = http;
        }

        public List<CartItem> GetCart()
        {
            var json = _http.HttpContext!.Session.GetString(CART_KEY);
            if (json == null) return new List<CartItem>();
            return JsonConvert.DeserializeObject<List<CartItem>>(json) ?? new List<CartItem>();
        }

        public void SaveCart(List<CartItem> cart)
        {
            var json = JsonConvert.SerializeObject(cart);
            _http.HttpContext!.Session.SetString(CART_KEY, json);
        }

        public void AddToCart(int productId, string name, decimal price, string? image)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductID == productId);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    ProductID = productId,
                    Name = name,
                    UnitPrice = price,
                    Quantity = 1,
                    ImageUrl = image
                });
            }
            else
            {
                item.Quantity++;
            }

            SaveCart(cart);
        }

        public void Remove(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductID == productId);
            if (item != null)
            {
                cart.Remove(item);
            }
            SaveCart(cart);
        }

        public void UpdateQuantity(int productId, int qty)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductID == productId);
            if (item != null)
            {
                item.Quantity = qty;
            }
            SaveCart(cart);
        }

        public void Clear()
        {
            SaveCart(new List<CartItem>());
        }
    }
}
