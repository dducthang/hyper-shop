using Microsoft.AspNetCore.Mvc;

namespace HyperShop.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
