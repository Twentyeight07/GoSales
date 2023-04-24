using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using GoSales.Models.ViewModels;
using GoSales.Utilities.Response;
using Domain.Interfaces;
using Entity;
using Microsoft.AspNetCore.Authorization;

namespace GoSales.Controllers
{
    [Authorize]

    public class BusinessController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBusinessService _businessService;

        public BusinessController(IMapper mapper, IBusinessService businessService)
        {
            _mapper = mapper;
            _businessService = businessService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            GenericResponse<VMBusiness> gResponse = new GenericResponse<VMBusiness>();

            try
            {
                VMBusiness vmBusiness = _mapper.Map<VMBusiness>(await _businessService.Get());

                gResponse.State = true;
                gResponse.Object = vmBusiness;
            }
            catch (Exception ex)
            {
                gResponse.State = true;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPost]
        public async Task<IActionResult> SaveChanges([FromForm] IFormFile logo, [FromForm] string model)
        {
            GenericResponse<VMBusiness> gResponse = new GenericResponse<VMBusiness>();

            try
            {
                VMBusiness vmBusiness = JsonConvert.DeserializeObject<VMBusiness>(model);

                string logoName = "";
                Stream logoStream = null;

                if(logo != null)
                {
                    string nameOnCode = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(logo.FileName);
                    logoName = String.Concat(nameOnCode, extension);
                    logoStream = logo.OpenReadStream();
                }

                Business editedBusiness = await _businessService.SaveChanges(_mapper.Map<Business>(vmBusiness), logoStream, logoName);

                vmBusiness = _mapper.Map<VMBusiness>(editedBusiness);

                gResponse.State = true;
                gResponse.Object = vmBusiness;
            }
            catch (Exception ex)
            {
                gResponse.State = true;
                gResponse.Message = ex.Message;
                throw;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}
