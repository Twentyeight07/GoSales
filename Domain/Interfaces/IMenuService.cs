using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Domain.Interfaces
{
    public interface IMenuService
    {
        Task<List<Menu>> GetAll(int userId);
    }
}
