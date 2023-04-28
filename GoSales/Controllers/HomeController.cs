using GoSales.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using GoSales.Models.ViewModels;
using GoSales.Utilities.Response;
using Domain.Interfaces;
using Entity;
using Firebase.Auth;
using Domain.Implementation;

namespace GoSales.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IUserService userService, IMapper mapper, INotificationService notificationService)
        {
            _userService = userService;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            List<VMNotification> vmNotification = _mapper.Map<List<VMNotification>>(await _notificationService.List());
            return StatusCode(StatusCodes.Status200OK, new { data = vmNotification });
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            GenericResponse<VMUser> res = new();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;

                string userId = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value)
                    .FirstOrDefault();

                VMUser user = _mapper.Map<VMUser>(await _userService.GetById(int.Parse(userId)));

                res.State = true;
                res.Object = user;
            }
            catch (Exception ex)
            {
                res.State = false;
                res.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, res);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProfile([FromBody] VMUser model)
        {
            GenericResponse<VMUser> res = new();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;

                string userId = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();

                Entity.User entity = _mapper.Map<Entity.User>(model);

                entity.UserId = int.Parse(userId);

                bool result = await _userService.SaveProfile(entity);

                res.State = result;
            }
            catch (Exception ex)
            {
                res.State = false;
                res.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, res);
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] VMChangePassword model)
        {
            GenericResponse<bool> res = new();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;

                string userId = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();


                bool result = await _userService.ChangePassword(int.Parse(userId), model.ActualPassword, model.NewPassword);

                res.State = result;
            }
            catch (Exception ex)
            {
                res.State = false;
                res.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, res);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Access");
        }
    }
}