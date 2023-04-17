using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DBContext;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Implementation
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly SalesDbContext _dbContext;

        public GenericRepository(SalesDbContext dbContext) 
        {
         _dbContext = dbContext;
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                TEntity entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(filter);
                return entity;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            try
            {
                _dbContext.Set<TEntity>().Add(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Edit(TEntity entity)
        {
            try
            {
                // This code can be changed to _dbContext.Update(entity) but for now I'm going to leave it
                _dbContext.Set<TEntity>().Update(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Delete(TEntity entity)
        {
            try
            {
                _dbContext.Remove(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IQueryable<TEntity>> Consult(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> queryEntity = filter == null ? _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().Where(filter);
            return queryEntity;
        }

    }
}
