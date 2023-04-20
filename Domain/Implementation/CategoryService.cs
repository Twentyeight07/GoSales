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
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _repository;

        public CategoryService(IGenericRepository<Category> repository)
        {
            _repository = repository;
        }

        public async Task<List<Category>> List()
        {
            IQueryable<Category> query = await _repository.Consult();
            return query.ToList();
        }

        public async Task<Category> Create(Category entity)
        {
            try
            {
                Category createdCategory = await _repository.Create(entity);
                if (createdCategory.CategoryId == 0)
                    throw new TaskCanceledException("No se pudo crear la categoría");

                return createdCategory;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Category> Edit(Category entity)
        {
            try
            {
                Category categoryFound = await _repository.Get(c => c.CategoryId == entity.CategoryId);
                categoryFound.Description = entity.Description;
                categoryFound.IsActive = entity.IsActive;

                bool res = await _repository.Edit(categoryFound);
                if(!res)
                    throw new TaskCanceledException("No se pudo modificar la categoría");

                return categoryFound;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Delete(int categoryId)
        {
            try
            {
                Category categoryFound = await _repository.Get(c => c.CategoryId == categoryId);

                if(categoryFound == null)
                    throw new TaskCanceledException("La categoría no existe");

                bool res = await _repository.Delete(categoryFound);

                return res;

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
