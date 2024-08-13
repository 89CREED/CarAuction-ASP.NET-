using Microsoft.AspNetCore.Mvc;

namespace AutaCH_MD.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
