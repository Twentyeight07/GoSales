using Microsoft.AspNetCore.Mvc;

namespace GoSales.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
