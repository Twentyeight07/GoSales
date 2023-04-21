using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Data.Interfaces
{
    public interface ISaleRepository:IGenericRepository<Sale>
    {
        Task<Sale> Record(Sale entity);
        Task<List<SaleDetail>> Report(DateTime StartDate, DateTime EndDate);
    }
}
