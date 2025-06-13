using System.Diagnostics;
using System.Security.Claims;
using HairCareStore.Data;
using HairCareStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairCareStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EshopContext _context;

        public HomeController(ILogger<HomeController> logger, EshopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Category).Include(p => p.Brand).ToList();
            if (products == null)
            {
                return NotFound();
            }

            ViewBag.FavProducts = products;
            ViewBag.Products = products;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Profile()
        {
            if (_context == null)
            {
                return NotFound();
            }
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _context.Users.FirstOrDefault(b => b.UserId == userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
<<<<<<< HEAD

=======
>>>>>>> cd7824df34d93c44b192ec11c62e2cc6bee927b6

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}