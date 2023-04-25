using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Data.Interfaces;
using Entity;

namespace Domain.Implementation
{
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<Menu> _menuRepository;
        private readonly IGenericRepository<RoleMenu> _roleRepository;
        private readonly IGenericRepository<User> _userRepository;

        public MenuService(IGenericRepository<Menu> menuRepository, IGenericRepository<RoleMenu> roleRepository, IGenericRepository<User> userRepository)
        {
            _menuRepository = menuRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;   
        }

        public async Task<List<Menu>> GetAll(int userId)
        {
            IQueryable<User> tbUser = await _userRepository.Consult(u => u.UserId == userId);
            IQueryable<RoleMenu> tbRoleMenu = await _roleRepository.Consult();
            IQueryable<Menu> tbMenu = await _menuRepository.Consult();

            IQueryable<Menu> parentMenu = (from u in tbUser
                                           join rm in tbRoleMenu on u.RoleId equals rm.Roleid 
                                           join m in tbMenu on rm.MenuId equals m.MenuId 
                                           join mparent in tbMenu on m.IdParentMenu equals mparent.MenuId 
                                           select mparent).Distinct().AsQueryable();

            IQueryable<Menu> childrenMenu = (from u in tbUser
                                             join rm in tbRoleMenu on u.RoleId equals rm.Roleid
                                             join m in tbMenu on rm.MenuId equals m.MenuId
                                             where m.MenuId != m.IdParentMenu
                                             select m).Distinct().AsQueryable();

            List<Menu> menuList = (from mparent in parentMenu
                                   select new Menu()
                                   {
                                       Description = mparent.Description,
                                       Icon = mparent.Icon,
                                       Controller = mparent.Controller,
                                       ActionPage = mparent.ActionPage,
                                       InverseIdParentMenuNavigation = (from mchildren in childrenMenu
                                                                        where mchildren.IdParentMenu == mparent.MenuId
                                                                        select mchildren).ToList()
                                   }).ToList();

            return menuList;

        }
    }
}
