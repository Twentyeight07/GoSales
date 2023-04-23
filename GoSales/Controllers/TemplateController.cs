using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using GoSales.Models.ViewModels;
using Domain.Interfaces;

namespace GoSales.Controllers
{
    public class TemplateController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBusinessService _businessService;
        private readonly ISaleService _saleService;

        public TemplateController(IMapper mapper, IBusinessService businessService, ISaleService saleService)
        {
            _mapper = mapper;
            _businessService = businessService;
            _saleService = saleService;
        }

        public IActionResult SendPassword(string email, string password)
        {
            ViewData["Email"] = email;
            ViewData["Password"] = password;
            ViewData["Url"] = $"{this.Request.Scheme}://{this.Request.Host}";

            return View();
        }

        public IActionResult RestorePassword(string password)
        {
            ViewData["Password"] = password;

            return View();
        }

        public async Task<IActionResult> SalePDF(string saleNumber)
        {
            VMSale vmSale = _mapper.Map<VMSale>(await _saleService.Detail(saleNumber));
            VMBusiness vmBusiness = _mapper.Map<VMBusiness>(await _businessService.Get());
            VMPDFSale model = new();

            model.Business = vmBusiness;
            model.Sale = vmSale;

            return View(model);
        }
    }
}
