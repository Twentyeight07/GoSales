using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using GoSales.Models.ViewModels;
using GoSales.Utilities.Response;
using Domain.Interfaces;
using Entity;

namespace GoSales.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRolesService _roleService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IRolesService roleService, IMapper mapper)
        {
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RoleList()
        {
            List<VMRole> vmRoleList = _mapper.Map<List<VMRole>>(await _roleService.List());
            return StatusCode(StatusCodes.Status200OK, vmRoleList);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<VMUser> vmUserList = _mapper.Map<List<VMUser>>(await _userService.List());
            return StatusCode(StatusCodes.Status200OK, new { data = vmUserList });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] IFormFile picture, [FromForm] string model)
        {
            GenericResponse<VMUser> gResponse = new GenericResponse<VMUser>();

            try
            {
                VMUser vmUser = JsonConvert.DeserializeObject<VMUser>(model);

                string pictureName = "";
                Stream streamPic = null;

                if (picture != null)
                {
                    string nameOnCode = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(picture.FileName);
                    pictureName = String.Concat(nameOnCode, extension);
                    streamPic = picture.OpenReadStream();
                }

                string emailTemplateUrl = $"{this.Request.Scheme}://{this.Request.Host}/Template/SendPassword?email=[email]&password=[password]";

                User createdUser = await _userService.Create(_mapper.Map<User>(vmUser), streamPic, pictureName, emailTemplateUrl);

                vmUser = _mapper.Map<VMUser>(createdUser);

                gResponse.State = true;
                gResponse.Object = vmUser;
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
            GenericResponse<VMUser> gResponse = new GenericResponse<VMUser>();

            try
            {
                VMUser vmUser = JsonConvert.DeserializeObject<VMUser>(model);

                string pictureName = "";
                Stream streamPic = null;

                if (picture != null)
                {
                    string nameOnCode = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(picture.FileName);
                    pictureName = String.Concat(nameOnCode, extension);
                    streamPic = picture.OpenReadStream();
                }

                User editedUser = await _userService.Edit(_mapper.Map<User>(vmUser), streamPic, pictureName);

                vmUser = _mapper.Map<VMUser>(editedUser);

                gResponse.State = true;
                gResponse.Object = vmUser;
            }
            catch (Exception ex)
            {
                gResponse.State = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int UserId)
        {
            GenericResponse<string> gResponse = new GenericResponse<String>();

            try
            {
                gResponse.State = await _userService.Delete(UserId);
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
