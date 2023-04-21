﻿using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using GoSales.Models.ViewModels;
using GoSales.Utilities.Response;
using Domain.Interfaces;
using Entity;

namespace GoSales.Controllers
{
    public class SaleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISaleDocTypeService _saleDocTypeService;
        private readonly ISaleService _saleService;

        public SaleController(IMapper mapper, ISaleDocTypeService saleDocTypeService, ISaleService saleService)
        {
            _mapper = mapper;
            _saleDocTypeService = saleDocTypeService;
            _saleService = saleService;
        }

        public IActionResult NewSale()
        {
            return View();
        }

        public IActionResult SaleHistory()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListSaleDocType()
        {
            List<VMSaleDocType> vmListDocType = _mapper.Map<List<VMSaleDocType>>(await _saleDocTypeService.List());

            return StatusCode(StatusCodes.Status200OK, vmListDocType);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(string search)
        {
            List<VMProduct> vmListProducts = _mapper.Map<List<VMProduct>>(await _saleService.GetAllProducts(search));

            return StatusCode(StatusCodes.Status200OK, vmListProducts);
        }


        [HttpPost]
        public async Task<IActionResult> RecordSale([FromBody] VMSale model)
        {
            GenericResponse<VMSale> gResponse = new GenericResponse<VMSale>();

            try
            {
                // Change this later!!!!
                model.UserId = 31;

                Sale createdSale = await _saleService.Record(_mapper.Map<Sale>(model));
                model = _mapper.Map<VMSale>(createdSale);

                gResponse.State = true;
                gResponse.Object = model;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message + ex.InnerException;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }


        [HttpGet]
        public async Task<IActionResult> History(string saleNumber, string starDate, string endDate)
        {
            List<VMSale> vmSaleHistory = _mapper.Map<List<VMSale>>(await _saleService.History(saleNumber, starDate, endDate));

            return StatusCode(StatusCodes.Status200OK, vmSaleHistory);
        }
    }
}
