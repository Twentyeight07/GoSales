using Microsoft.AspNetCore.Mvc;
using GoSales.Models.ViewModels;
using Domain.Interfaces;
using Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace GoSales.Controllers
{
    public class AccessController : Controller
    {
        private readonly IUserService _userService;

        public AccessController(IUserService userService)
        {
            _userService = userService;
        }

        // Method to display the login view
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if(claimUser.Identity.IsAuthenticated)
            {
                // If the user is authenticated, we redirect him to the Index
                return RedirectToAction("Index", "Home");

            }

            return View();
        }

        public IActionResult RestorePassword()
        {

            return View();
        }

        // Method to process the login form
        [HttpPost]
        public async Task<IActionResult> Login(VMUserLogin model)
        {
            // Get the user found based on the entered credentials
            User userFound = await _userService.GetByCredentials(model.Email, model.Password);

            // If user is not found, display an error message in the login view
            if (userFound == null)
            {
                ViewData["Message"] = "Usuario o contraseña incorrecto(s)";
                return View();
            }
            ViewData["Message"] = null;

            // Create a list of claims to store user information
            List<Claim> claims = new List<Claim>()
            {
                //All this parameters come from the DataBase, more especific from the UserLogin table
                new Claim(ClaimTypes.Name, userFound.Name),
                new Claim(ClaimTypes.NameIdentifier, userFound.UserId.ToString()),
                new Claim(ClaimTypes.Role, userFound.RoleId.ToString()),
                new Claim("picUrl", userFound.PicUrl),
            };

            // Create a claims identity for the user
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Configure authentication properties
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = model.KeepSectionAlive
            };

            // Sign in the user to the system using cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            // Redirect to the home page after successful login
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> RestorePassword(VMUserLogin model)
        {
            try
            {
                string emailTemplateUrl = $"{this.Request.Scheme}://{this.Request.Host}/Template/RestorePassword?password=$[password]";

                bool res = await _userService.RestorePassword(model.Email, emailTemplateUrl);

                if (res)
                {
                    ViewData["Message"] = "Su contraseña ha sido reestablecida, revisar su bandeja de entrada";
                    ViewData["ErrorMessage"] = null;
                }
                else
                {
                    ViewData["ErrorMessage"] = "En estos momentos no podemos completar su solicitud, por favor intente de nuevo más tarde.";
                    ViewData["Message"] = null;
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message + ex.InnerException;
                ViewData["Message"] = null;
            }

            return View();
        }
    }
}
