using Microsoft.AspNetCore.Mvc;
using GoSales.Models.ViewModels;
using GoSales.Utilities.Response;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace GoSales.Controllers
{
    [Authorize]

    public class DashBoardController : Controller
    {
        private readonly IDashBoardService _dashboardService;

        public DashBoardController(IDashBoardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetResume()
        {
                GenericResponse<VMDashBoard> gResponse = new();
            try
            {
                VMDashBoard vmDashboard = new();

                vmDashboard.TotalSales = await _dashboardService.TotalLastWeekSales();
                vmDashboard.TotalIncome = await _dashboardService.TotalLastWeekIncome();
                vmDashboard.TotalProducts = await _dashboardService.TotalProducts();
                vmDashboard.TotalCategories = await _dashboardService.TotalCategories();

                List<VMWeekSales> listWeekSales = new();
                List<VMWeekProducts> listWeekProducts = new();

                foreach(KeyValuePair<string,int> item in await _dashboardService.LastWeekSales())
                {
                    listWeekSales.Add(new VMWeekSales()
                    {
                        Date = item.Key,
                        Total = item.Value
                    });
                }

                foreach (KeyValuePair<string, int> item in await _dashboardService.TopProductsLastWeek())
                {
                    listWeekProducts.Add(new VMWeekProducts()
                    {
                        Product = item.Key,
                        Quantity = item.Value
                    });
                }

                vmDashboard.LastWeekSales = listWeekSales;
                vmDashboard.LastWeekTopProducts = listWeekProducts;

                gResponse.State = true;
                gResponse.Object = vmDashboard;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

    }
}
