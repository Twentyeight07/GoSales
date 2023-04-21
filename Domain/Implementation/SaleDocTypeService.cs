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
    public class SaleDocTypeService : ISaleDocTypeService
    {
        private readonly IGenericRepository<SaleDocType> _repository;

        public SaleDocTypeService(IGenericRepository<SaleDocType> repository)
        {
            _repository = repository;
        }

        public async Task<List<SaleDocType>> List()
        {
            IQueryable<SaleDocType> query = await _repository.Consult();
            return query.ToList();
        }
    }
}
