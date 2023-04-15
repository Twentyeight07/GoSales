using Microsoft.AspNetCore.Mvc;

namespace GoSales.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
