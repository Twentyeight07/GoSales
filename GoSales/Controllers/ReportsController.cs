using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using GoSales.Models.ViewModels;
using Domain.Interfaces;
using Domain.Implementation;
using Microsoft.AspNetCore.Authorization;

namespace GoSales.Controllers
{
    [Authorize]

    public class ReportsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISaleService _saleService;

        public ReportsController(IMapper mapper, ISaleService saleService)
        {
            _mapper = mapper;
            _saleService = saleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SaleReport(string startDate, string endDate)
        {
            List<VMSaleReport> vmSaleReport = _mapper.Map<List<VMSaleReport>>(await _saleService.SaleReport(startDate, endDate));

            return StatusCode(StatusCodes.Status200OK, new { data = vmSaleReport });
        }
    }
}
