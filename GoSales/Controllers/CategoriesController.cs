using Microsoft.AspNetCore.Mvc;

namespace GoSales.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
