using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Data.Interfaces;
using Entity;
using System.Globalization;
using Data.Implementation;

namespace Domain.Implementation
{
    public class DashBoardService : IDashBoardService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IGenericRepository<SaleDetail> _saleDetailRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private DateTime StartDate = DateTime.Now;

        public DashBoardService(ISaleRepository saleRepository, IGenericRepository<SaleDetail> saleDetailRepository, IGenericRepository<Category> categoryRepository, IGenericRepository<Product> productRepository)
        {
            _saleRepository = saleRepository;
            _saleDetailRepository = saleDetailRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;

            StartDate = StartDate.AddDays(-7);
        }

        public async Task<int> TotalLastWeekSales()
        {
            try
            {
                IQueryable<Sale> query = await _saleRepository.Consult(s => s.RegistryDate.Value.Date >= StartDate.Date);
                int total = query.Count();
                return total;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> TotalLastWeekIncome()
        {
            try
            {
                IQueryable<Sale> query = await _saleRepository.Consult(s => s.RegistryDate.Value.Date >= StartDate.Date);
                decimal res = query.Select(s => s.Total).Sum(s => s.Value);

                return Convert.ToString(res, new CultureInfo("es-US"));

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> TotalProducts()
        {
            try
            {
                IQueryable<Product> query = await _productRepository.Consult();
                int total = query.Count();

                return total;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> TotalCategories()
        {
            try
            {
                IQueryable<Category> query = await _categoryRepository.Consult();
                int total = query.Count();

                return total;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Dictionary<string, int>> LastWeekSales()
        {
            try
            {
                IQueryable<Sale> query = await _saleRepository.Consult(s => s.RegistryDate.Value.Date >= StartDate.Date);

                Dictionary<string, int> res = query.GroupBy(s => s.RegistryDate.Value.Date).OrderByDescending(g => g.Key).Select(sd => new {date = sd.Key.ToString("dd/MM/yyyy"), total = sd.Count()}).ToDictionary(keySelector: r => r.date, elementSelector: r => r.total);

                return res;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Dictionary<string, int>> TopProductsLastWeek()
        {
            try
            {
                IQueryable<SaleDetail> query = await _saleDetailRepository.Consult();

                Dictionary<string, int> res = query.Include(s => s.Sale).Where(sd => sd.Sale.RegistryDate.Value.Date >= StartDate.Date).GroupBy(sd => sd.ProductDescription).OrderByDescending(g => g.Count()).Select(sd => new { product = sd.Key, total = sd.Count() }).Take(4).ToDictionary(keySelector: r => r.product, elementSelector: r => r.total);

                return res;

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
