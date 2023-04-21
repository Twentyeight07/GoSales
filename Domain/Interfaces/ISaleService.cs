using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Domain.Interfaces
{
    public interface ISaleService
    {
        Task<List<Product>> GetAllProducts(string search);

        Task<Sale> Record(Sale entity);

        Task<List<Sale>> History(string saleNumber, string startDate, string endDate);

        Task<Sale> Detail(string saleNumber);

        Task<List<SaleDetail>> SaleReport(string startDate, string endDate);
    }
}
