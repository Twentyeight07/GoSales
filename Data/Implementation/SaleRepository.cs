using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data.DBContext;
using Data.Interfaces;
using Entity;

namespace Data.Implementation
{
    public class SaleRepository : GenericRepository<Sale>, ISaleRepository
    {
        private readonly SalesDbContext _dbContext;

        public SaleRepository(SalesDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Sale> Registry(Sale entity)
        {
            Sale generatedSale = new Sale();

            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach(SaleDetail sd in entity.SaleDetails)
                    {
                        Product product_found = _dbContext.Products.Where(p=> p.ProductId == sd.ProductId).FirstOrDefault();

                        product_found.Stock = product_found.Stock - sd.Quantity;
                        _dbContext.Products.Update(product_found);
                    }
                    await _dbContext.SaveChangesAsync();

                    CorrelativeNumber correlative = _dbContext.CorrelativeNumbers.Where(n=> n.Management == "sale").First();

                    correlative.LastNumber = correlative.LastNumber + 1;
                    correlative.UpdateDate = DateTime.Now;

                    _dbContext.CorrelativeNumbers.Update(correlative);
                    await _dbContext.SaveChangesAsync();

                    string ceros = string.Concat(Enumerable.Repeat("0", correlative.DigitsQuantity.Value));
                    string saleNumber = ceros + correlative.LastNumber.ToString();
                    saleNumber = saleNumber.Substring(saleNumber.Length - correlative.DigitsQuantity.Value, correlative.DigitsQuantity.Value);
                    entity.SaleNumber = saleNumber;

                    await _dbContext.Sale.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();

                    generatedSale = entity;

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return generatedSale;
        }

        public async Task<List<Sale>> Report(DateTime StartDate, DateTime EndDate)
        {
            //List<SaleDetail> resumeList = _dbContext.SaleDetails.Include(s => s.SaleIdNavigation).ThenInclude()
        }
    }
}
