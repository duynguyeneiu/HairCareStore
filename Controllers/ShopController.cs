using HairCareStore.Data;
using HairCareStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairCareStore.Controllers
{
    public class ShopController : Controller
    {

        private readonly EshopContext _context;

        public ShopController(EshopContext context)
        {

            _context = context;
        }

        public IActionResult Index(int? id, string searchString)
        {
            IQueryable<Product>
    productsQuery = _context.Products
    .Include(p => p.Category)
    .Include(p => p.Brand);
            if (id.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == id);
                ViewBag.CurrentCategory = _context.Categories.Find(id);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                productsQuery = productsQuery.Where(p =>
                    p.Name.Contains(searchString) ||
                    p.Description.Contains(searchString));
            }

            ViewBag.Products = productsQuery.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.CurrentSearch = searchString;

            return View();
        }



    }
}