using Microsoft.AspNetCore.Mvc;

namespace GoSales.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
