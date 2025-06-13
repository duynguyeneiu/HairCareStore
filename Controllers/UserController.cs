using HairCareStore.Data;
using HairCareStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HairCareStore.Controllers
{
    public class UserController : Controller
    {
        private readonly EshopContext _context;

        public UserController(EshopContext context)
        {
            _context = context;
        }


      
        public ActionResult Index()
        {
            return View();
        }


       

        //[HttpGet]
        //public IActionResult Detail(int id)
        //{
        //    var product = _context.Products.Include(p => p.Category).Include(p => p.Brand).FirstOrDefault(p => p.ProductId == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return PartialView("_ProductDetailPartial", product);
        //}


        public ActionResult Table()
        {
            var users = _context.Users.Include(u=>u.Role).ToList();
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile ImageFile)
        {
            ModelState.Remove("Avatar");

            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Products", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    product.ImageUrl = fileName;
                }
                else
                {
                    product.ImageUrl = "default.jpg";
                }
                product.CreatedDate = DateTime.Now;

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Table", "Product");

            }
            else
            {
                ViewBag.Brand = _context.Brands.Select(b => new SelectListItem
                {
                    Value = b.BrandId.ToString(),
                    Text = b.Name
                }).ToList();
                ViewBag.Category = _context.Categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList();
                return View(product);
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(b => b.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Brand = _context.Brands.Select(a => new SelectListItem
            {
                Value = a.BrandId.ToString(),
                Text = a.Name
            }).ToList();

            ViewBag.Category = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToList();
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? ImageFile)
        {
            var CurrentProduct = _context.Products.FirstOrDefault(b => b.ProductId == id);
            if (CurrentProduct == null)
            {
                return NotFound();
            }
            ModelState.Remove("ImageUrl");


            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Products", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    CurrentProduct.ImageUrl = fileName;
                }

                CurrentProduct.Name = product.Name;
                CurrentProduct.Description = product.Description;
                CurrentProduct.BarCode = product.BarCode;


                CurrentProduct.CategoryId = product.CategoryId;
                CurrentProduct.BrandId = product.BrandId;
                CurrentProduct.Price = product.Price;
                CurrentProduct.Quantity = product.Quantity;

                await _context.SaveChangesAsync();
                return RedirectToAction("Table", "Product");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine("Fix here: " + error); // Hoặc log ra file
                }

                ViewBag.Brand = _context.Brands.Select(a => new SelectListItem
                {
                    Value = a.BrandId.ToString(),
                    Text = a.Name
                }).ToList();

                ViewBag.Category = _context.Categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList();

                return View(CurrentProduct);
            }
        }

        // GET: ProductController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var product = await _context.Products.FirstOrDefaultAsync(b => b.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);

        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Table", "Product");
        }
    }
}
