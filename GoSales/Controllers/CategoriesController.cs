using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using GoSales.Models.ViewModels;
using GoSales.Utilities.Response;
using Domain.Interfaces;
using Entity;
using Microsoft.AspNetCore.Authorization;

namespace GoSales.Controllers
{
        [Authorize]
    public class CategoriesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public CategoriesController(IMapper mapper, ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<VMCategory> vmCategoryList = _mapper.Map<List<VMCategory>>(await _categoryService.List());
            return StatusCode(StatusCodes.Status200OK, new { data = vmCategoryList });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VMCategory model)
        {
            GenericResponse<VMCategory> gResponse = new GenericResponse<VMCategory>();

            try
            {
                Category createdCategory = await _categoryService.Create(_mapper.Map<Category>(model));
                model = _mapper.Map<VMCategory>(createdCategory);

                gResponse.State = true;
                gResponse.Object = model;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
                
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] VMCategory model)
        {
            GenericResponse<VMCategory> gResponse = new GenericResponse<VMCategory>();

            try
            {
                Category editedCategory = await _categoryService.Edit(_mapper.Map<Category>(model));
                model = _mapper.Map<VMCategory>(editedCategory);

                gResponse.State = true;
                gResponse.Object = model;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;

            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int categoryId)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.State = await _categoryService.Delete(categoryId);
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
