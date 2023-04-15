using Microsoft.AspNetCore.Mvc;

namespace GoSales.Controllers
{
    public class BusinessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
