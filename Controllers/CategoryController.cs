using HairCareStore.Data;
using HairCareStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HairCareStore.Controllers
{
    public class CategoryController : Controller
    {

        private readonly EshopContext _context;

        public CategoryController(EshopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Table()
        {
            var categories = _context.Categories.ToList();
            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
           
        }
         
        public ActionResult Create()
        {

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, IFormFile ImageFile)
        {
            ModelState.Remove("Avatar");

            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Categories", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    category.Avatar = fileName;
                }
                else
                {
                    category.Avatar = "default.jpg";
                }
                category.CreatedDate = DateTime.Now;

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Table", "Category");

            }
            else
            {
               
                return View(category);
            }
        }

         

        public ActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(b => b.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category, IFormFile? ImageFile)
        {
            var CurrentCategory = _context.Categories.FirstOrDefault(b => b.CategoryId == id);
            if (CurrentCategory == null)
            {
                return NotFound();
            }
            ModelState.Remove("Avatar");


            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Categories", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    CurrentCategory.Avatar = fileName;
                }

                CurrentCategory.Name = category.Name;
               

                await _context.SaveChangesAsync();
                return RedirectToAction("Table", "Category");
            }
            else
            {
                
                return View(CurrentCategory);
            }
        }

         
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var category = await _context.Categories.FirstOrDefaultAsync(b => b.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Table", "Category");
        }
    }
}
