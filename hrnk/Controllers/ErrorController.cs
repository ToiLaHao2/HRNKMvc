using Microsoft.AspNetCore.Mvc;

namespace hrnk.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AccessDenined()
        {
            return View();
        }
    }
}
