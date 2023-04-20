using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Domain.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> List();
        Task<Product> Create(Product entity, Stream picture = null, string picName = "");
        Task<Product> Edit(Product entity, Stream picture = null, string picName = "");
        Task<bool> Delete(int productId);
    }
}
