using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Domain.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> List();
        Task<Category> Create(Category entity);
        Task<Category> Edit(Category entity);
        Task<bool> Delete(int categoryId);
    }
}
