using HairCareStore.Data;
using HairCareStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairCareStore.Controllers
{
    public class BrandController : Controller
    {
        private readonly EshopContext _context;

        public BrandController(EshopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Table()
        {
            var brands = _context.Brands.ToList();
            if (brands == null)
            {
                return NotFound();
            }
            return View(brands);
        }

        public ActionResult Create()
        {

            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand, IFormFile ImageFile)
        {
            ModelState.Remove("Logo");

            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Brands", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    brand.Logo = fileName;
                }
                else
                {
                    brand.Logo = "default.jpg";
                }
               

                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction("Table", "Brand");

            }
            else
            {

                return View(brand);
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var brand = _context.Brands.FirstOrDefault(b => b.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Brand brand, IFormFile? ImageFile)
        {
            var CurrentBrand = _context.Brands.FirstOrDefault(b => b.BrandId == id);
            if (CurrentBrand == null)
            {
                return NotFound();
            }
            ModelState.Remove("Logo");


            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Brands", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    CurrentBrand.Logo = fileName;
                }

                CurrentBrand.Name = brand.Name;
                CurrentBrand.Description = brand.Description;



                await _context.SaveChangesAsync();
                return RedirectToAction("Table", "Brand");
            }
            else
            {

                return View(CurrentBrand);
            }
        }

        // GET: ProductController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var brand = await _context.Brands.FirstOrDefaultAsync(b => b.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);

        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction("Table", "Brand");
        }
    }
}
