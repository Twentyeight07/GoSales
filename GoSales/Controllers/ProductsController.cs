using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using GoSales.Models.ViewModels;
using GoSales.Utilities.Response;
using Domain.Interfaces;
using Entity;

namespace GoSales.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductsController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<VMProduct> vmProductList = _mapper.Map<List<VMProduct>>(await _productService.List());

            return StatusCode(StatusCodes.Status200OK, new { data = vmProductList });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] IFormFile picture, [FromForm] string model)
        {
            GenericResponse<VMProduct> gResponse = new GenericResponse<VMProduct>();

            try
            {
                VMProduct vmProduct = JsonConvert.DeserializeObject<VMProduct>(model);

                string picName = "";
                Stream streamPicture = null;

                if(picture != null)
                {
                    string nameOnCode = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(picture.FileName);
                    picName = String.Concat(nameOnCode, extension);
                    streamPicture = picture.OpenReadStream();
                }

                Product createdProduct = await _productService.Create(_mapper.Map<Product>(vmProduct), streamPicture, picName);
                vmProduct = _mapper.Map<VMProduct>(createdProduct);

                gResponse.State = true;
                gResponse.Object = vmProduct;

            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }


        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] IFormFile picture, [FromForm] string model)
        {
            GenericResponse<VMProduct> gResponse = new GenericResponse<VMProduct>();

            try
            {
                VMProduct vmProduct = JsonConvert.DeserializeObject<VMProduct>(model);

                string picName = "";
                Stream streamPicture = null;

                if (picture != null)
                {
                    string nameOnCode = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(picture.FileName);
                    picName = String.Concat(nameOnCode, extension);
                    streamPicture = picture.OpenReadStream();
                }

                Product editedProduct = await _productService.Edit(_mapper.Map<Product>(vmProduct), streamPicture, picName);
                vmProduct = _mapper.Map<VMProduct>(editedProduct);

                gResponse.State = true;
                gResponse.Object = vmProduct;

            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int ProductId)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.State = await _productService.Delete(ProductId);


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
