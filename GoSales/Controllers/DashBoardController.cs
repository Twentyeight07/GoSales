using Microsoft.AspNetCore.Mvc;

namespace GoSales.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
