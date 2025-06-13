using Microsoft.AspNetCore.Mvc;

namespace HairCareStore.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
