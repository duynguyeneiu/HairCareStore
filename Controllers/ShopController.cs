using HairCareStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairCareStore.Controllers
{
    public class ShopController : Controller
    {
     
        private readonly EshopContext _context;

        public ShopController( EshopContext context)
        {
            
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Category).Include(p => p.Brand).ToList();
            if (products == null)
            {
                return NotFound();
            }


            ViewBag.Products = products;


            var categories = _context.Categories.ToList();
            if (categories == null)
            {
                return NotFound();
            }
            ViewBag.Categories = categories;


            return View();
        }


    }
}
