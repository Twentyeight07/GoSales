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

namespace Domain.Implementation
{
    public class SaleService : ISaleService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly ISaleRepository _saleRepository;

        public SaleService(IGenericRepository<Product> productRepository, ISaleRepository saleRepository)
        {
            _productRepository = productRepository;
            _saleRepository = saleRepository;
        }

        public async Task<List<Product>> GetAllProducts(string search)
        {
            IQueryable<Product> query = await _productRepository.Consult(p => p.IsActive == true && p.Stock > 0 && String.Concat(p.BarCode,p.Brand,p.Description).Contains(search));

            return query.Include(c => c.Category).ToList();
        }

        public async Task<Sale> Record(Sale entity)
        {
            try
            {
                return await _saleRepository.Record(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<Sale>> History(string saleNumber, string startDate, string endDate)
        {
            IQueryable<Sale> query = await _saleRepository.Consult();
            startDate = startDate is null ? "" : startDate;
            endDate = endDate is null ? "" : endDate;

            if(startDate != "" && endDate != "")
            {
                DateTime start_date = DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("es-US"));
                DateTime end_date = DateTime.ParseExact(endDate, "dd/MM/yyyy", new CultureInfo("es-US"));

                return query.Where(s => s.RegistryDate.Value.Date >= start_date && s.RegistryDate.Value.Date <= end_date).Include(sdt => sdt.SaleDocType).Include(u => u.User).Include(sd => sd.SaleDetail).ToList();
            }
            else
            {
                return query.Where(s => s.SaleNumber == saleNumber).Include(sdt => sdt.SaleDocType).Include(u => u.User).Include(sd => sd.SaleDetail).ToList();
            }

        }

        public async Task<Sale> Detail(string saleNumber)
        {
            IQueryable<Sale> query = await _saleRepository.Consult(v => v.SaleNumber == saleNumber);

            return query.Include(sdt => sdt.SaleDocType).Include(u => u.User).Include(sd => sd.SaleDetail).First();
        }

        public async Task<List<SaleDetail>> SaleReport(string startDate, string endDate)
        {
            DateTime start_date = DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("es-US"));
            DateTime end_date = DateTime.ParseExact(endDate, "dd/MM/yyyy", new CultureInfo("es-US"));

            List<SaleDetail> list = await _saleRepository.Report(start_date, end_date);

            return list;
        }
    }
}
