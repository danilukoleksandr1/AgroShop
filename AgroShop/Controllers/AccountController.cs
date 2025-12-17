using AgroShop.Web.Data;
using AgroShop.Web.Models;
using AgroShop.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AgroShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AgroShopContext _context;

        public AccountController(AgroShopContext context)
        {
            _context = context;
        }

        // ================= REGISTER =================
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (await _context.Users.AnyAsync(u => u.Email == vm.Email))
            {
                ModelState.AddModelError("Email", "Користувач з таким Email вже існує");
                return View(vm);
            }

            if (await _context.Users.AnyAsync(u => u.Username == vm.Username))
            {
                ModelState.AddModelError("Username", "Користувач з таким ім'ям вже існує");
                return View(vm);
            }

            var user = new User
            {
                Username = vm.Username,
                Email = vm.Email,
                PasswordHash = Hash(vm.Password),
                RoleID = 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await SignIn(user);
            return RedirectToAction("Profile");
        }

        // ================= LOGIN =================
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == vm.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Користувача з таким Email не існує");
                return View(vm);
            }

            if (user.PasswordHash != Hash(vm.Password))
            {
                ModelState.AddModelError("Password", "Невірний пароль");
                return View(vm);
            }

            await SignIn(user);
            return RedirectToAction("Profile");
        }

        // ================= PROFILE =================
        public async Task<IActionResult> Profile()
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("Login");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var orders = await _context.Orders
                .Include(o => o.Status)
                .Where(o => o.UserID == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var vm = new ProfileViewModel
            {
                Username = User.Identity.Name!,
                Email = User.FindFirst(ClaimTypes.Email)!.Value,
                Orders = orders
            };

            return View(vm);
        }


        // ================= LOGOUT =================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }

        // ================= HELPERS =================
        private async Task SignIn(User user)
        {
            var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
    new Claim(ClaimTypes.Name, user.Username),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Role, user.RoleID == 2 ? "Admin" : "User")
};


            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);
        }

        private string Hash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(
                sha.ComputeHash(Encoding.UTF8.GetBytes(input))
            );
        }
    }
}
