using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairCareStore.Controllers
{
    public class AdminController : Controller
    {
<<<<<<< HEAD

=======
>>>>>>> cd7824df34d93c44b192ec11c62e2cc6bee927b6
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}