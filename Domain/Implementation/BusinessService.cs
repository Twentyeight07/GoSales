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
    public class BusinessService : IBusinessService
    {
        private readonly IGenericRepository<Business> _repository;
        private readonly IFireBaseService _fireBaseService;

        public BusinessService(IGenericRepository<Business> repository, IFireBaseService fireBaseService)
        {
            _repository = repository;
            _fireBaseService = fireBaseService;
        }

        public async Task<Business> Get()
        {
            try
            {
                Business businessFound = await _repository.Get(n => n.BusinessId == 1);
                return businessFound;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Business> SaveChanges(Business entity, Stream logo = null, string logoName = "")
        {
            try
            {
                Business businessFound = await _repository.Get(n => n.BusinessId == 1);

                businessFound.DocNumber = entity.DocNumber;
                businessFound.Name = entity.Name;
                businessFound.Email = entity.Email;
                businessFound.Address = entity.Address;
                businessFound.Phone = entity.Address;
                businessFound.TaxRate = entity.TaxRate;
                businessFound.CurrencySymbol = entity.CurrencySymbol;

                businessFound.LogoName = businessFound.LogoName == "" ? logoName : businessFound.LogoName;

                if(logo != null)
                {
                    string logoUrl = await _fireBaseService.UploadStorage(logo, "logo_folder", businessFound.LogoName);
                    businessFound.LogoUrl = logoUrl;
                }

                await _repository.Edit(businessFound);
                return businessFound;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
