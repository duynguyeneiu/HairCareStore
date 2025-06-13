using HairCareStore.Data;
using HairCareStore.Models;
using Microsoft.AspNetCore.Authorization;
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

         
        public ActionResult Create()
        {

            ViewBag.Role = _context.Role.Select(b => new SelectListItem
            {
                Value = b.RoleId.ToString(),
                Text = b.Name
            }).ToList();


            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user, IFormFile ImageFile)
        {
            ModelState.Remove("Avatar");
            ModelState.Remove("Role");

            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Users", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    user.Avatar = fileName;
                }
                else
                {
                    user.Avatar = "default.jpg";
                }
                user.CreatedDate = DateTime.Now;
                

                _context.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Table", "User");

            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine("Fix here: " + error); // Hoặc log ra file
                }

                ViewBag.Role = _context.Role.Select(b => new SelectListItem
                {
                    Value = b.RoleId.ToString(),
                    Text = b.Name
                }).ToList();
                return View(user);
            }
        }
         
        public ActionResult Edit(int id)
        {
            var user = _context.Users.FirstOrDefault(b => b.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Role = _context.Role.Select(b => new SelectListItem
            {
                Value = b.RoleId.ToString(),
                Text = b.Name
            }).ToList();
            return View(user);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user, IFormFile? ImageFile)
        {
            var CurrentUser = _context.Users.FirstOrDefault(b => b.UserId == id);
            if (CurrentUser == null)
            {
                return NotFound();
            }
            ModelState.Remove("Avatar");
            ModelState.Remove("Role");
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Users", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    CurrentUser.Avatar = fileName;
                }

                CurrentUser.Fullname = user.Fullname;
                CurrentUser.Description = user.Description;
                CurrentUser.Email = user.Email;
                CurrentUser.Phone = user.Phone;
                CurrentUser.Address = user.Address;
                CurrentUser.RoleId = user.RoleId;


                await _context.SaveChangesAsync();
                return RedirectToAction("Table", "User");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine("Fix here: " + error); // Hoặc log ra file
                }


                ViewBag.Role = _context.Role.Select(b => new SelectListItem
                {
                    Value = b.RoleId.ToString(),
                    Text = b.Name
                }).ToList();
               

                return View(CurrentUser);
            }
        }

         
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var user = await _context.Users.FirstOrDefaultAsync(b => b.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);

        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Table", "User");
        }
    }
}
