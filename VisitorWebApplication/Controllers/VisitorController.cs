using Microsoft.AspNetCore.Mvc;

namespace VisitorWebApplication.Controllers
{
    public class VisitorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
