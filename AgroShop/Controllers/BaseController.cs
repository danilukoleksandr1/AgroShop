using AgroShop.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgroShop.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly CartService _cartService;

        public BaseController(CartService cartService)
        {
            _cartService = cartService;
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var cart = _cartService.GetCart();
            ViewBag.CartCount = cart.Sum(x => x.Quantity);
        }
    }
}
