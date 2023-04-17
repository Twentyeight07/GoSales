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
    public class RoleService : IRolesService
    {
        private readonly IGenericRepository<Role> _repository;

        public RoleService(IGenericRepository<Role> repository)
        {
            _repository = repository;
        }

        public async Task<List<Role>> List()
        {
            IQueryable<Role> query = await _repository.Consult();

            return query.ToList();
        }
    }
}
