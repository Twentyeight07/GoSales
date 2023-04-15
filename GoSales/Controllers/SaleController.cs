using Microsoft.AspNetCore.Mvc;

namespace GoSales.Controllers
{
    public class SaleController : Controller
    {
        public IActionResult NewSale()
        {
            return View();
        }

        public IActionResult SaleHistory()
        {
            return View();
        }
    }
}
