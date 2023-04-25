using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GoSales.Models.ViewModels;
using Domain.Interfaces;

namespace GoSales.Utilities.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;

        public MenuViewComponent(IMenuService menuService, IMapper mapper)
        {
            _menuService = menuService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            List<VMMenu> listMenu;

            if (claimUser.Identity.IsAuthenticated)
            {
            string userId = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault();

                listMenu = _mapper.Map<List<VMMenu>>(await _menuService.GetAll(int.Parse(userId)));
            }
            else
            {
                listMenu = new List<VMMenu> { };
            }

            return View(listMenu);

        }

    }
}
