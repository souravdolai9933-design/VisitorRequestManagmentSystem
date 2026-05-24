using Microsoft.AspNetCore.Mvc;

namespace VisitorWebApplication.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult login()
        {
            return View();
        }
    }
}
