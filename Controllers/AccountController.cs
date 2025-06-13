using HairCareStore.Data;
using HairCareStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HairCareStore.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly EshopContext _context;

        public AccountController(EshopContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users
                .Include(u => u.UserRole)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.Email == email);

            if ((user == null || password != user.Password))
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim("RoleId", user.RoleId.ToString())
                };
            claims.AddRange(user.UserRole.Select(ur => new Claim(ClaimTypes.Role, ur.Role.Name)));
            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.SignInAsync("CookieAuth", claimsPrincipal);
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View(new User());
        }

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(string FullName, string Password, string Email, string ConfirmPassword)
        {
            var model = new User();

            // Kiểm tra email đã tồn tại
            if (_context.Users.Any(u => u.Email == Email))
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng.");
                return View(model);
            }

            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Mật khẩu và xác nhận mật khẩu không khớp.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                model.Fullname = FullName;
                model.Avatar = "john_avatar.jpg";
                model.Password = Password;
                model.Email = Email;
                model.Status = 0;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;
                model.IsDeleted = false;
                model.IsLocked = false;
                model.RoleId = 3;
                _context.Users.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

    }
}
