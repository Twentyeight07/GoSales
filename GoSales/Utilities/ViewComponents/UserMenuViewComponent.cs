using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoSales.Utilities.ViewComponents
{
    public class UserMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            string userName = "";
            string userPicUrl = "";

            if (claimUser.Identity.IsAuthenticated)
            {
                userName = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();

                userPicUrl = ((ClaimsIdentity)claimUser.Identity).FindFirst("picUrl").Value;
            }

            ViewData["userName"] = userName;
            ViewData["userPicUrl"] = userPicUrl;

            return View();
        }

    }
}
