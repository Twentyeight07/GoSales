using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDashBoardService
    {
        Task<int> TotalLastWeekSales();
        Task<string> TotalLastWeekIncome();
        Task<int> TotalProducts();
        Task<int> TotalCategories();
        Task<Dictionary<string, int>> LastWeekSales();
        Task<Dictionary<string, int>> TopProductsLastWeek();
    }
}
