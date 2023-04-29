using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Data.Interfaces;
using Entity;

namespace Domain.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _repository;
        private readonly IFireBaseService _fireBaseService;

        public ProductService(IGenericRepository<Product> repository, IFireBaseService fireBaseService, IUtilitiesService UtilitiesService)
        {
            _repository = repository;
            _fireBaseService = fireBaseService;
        }

        public async Task<List<Product>> List()
        {
            IQueryable<Product> query = await _repository.Consult();
            return query.Include(c => c.Category).ToList();
        }


        public async Task<Product> Create(Product entity, Stream picture = null, string picName = "")
        {
            Product productExists = await _repository.Get(p => p.BarCode == entity.BarCode);

            if (productExists != null)
                throw new TaskCanceledException("El código de barra ya existe");

            try
            {
                entity.PicName = picName;
                
                if(picture != null)
                {
                    string picUrl = await _fireBaseService.UploadStorage(picture, "product_folder", picName);
                    entity.PicUrl = picUrl;
                }

                Product createdProduct = await _repository.Create(entity);

                if(createdProduct.ProductId == 0)
                throw new TaskCanceledException("No se pudo crear el producto");

                IQueryable<Product> query = await _repository.Consult(p => p.ProductId == createdProduct.ProductId);

                createdProduct = query.Include(c => c.Category).First();

                return createdProduct;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Product> Edit(Product entity, Stream picture = null, string picName = "")
        {
            Product productExists = await _repository.Get(p => p.BarCode == entity.BarCode && p.ProductId != entity.ProductId);

            if(productExists != null)
                throw new TaskCanceledException("El código de barra ya está asignado a otro producto");

            try
            {
                IQueryable<Product> productQuery = await _repository.Consult(p => p.ProductId == entity.ProductId);

                Product productToEdit = productQuery.First();

                productToEdit.BarCode = entity.BarCode;
                productToEdit.Brand = entity.Brand;
                productToEdit.Description = entity.Description;
                productToEdit.CategoryId = entity.CategoryId;
                productToEdit.Stock = entity.Stock;
                productToEdit.Price = entity.Price;
                productToEdit.IsActive = entity.IsActive;

                if(productToEdit.PicName == String.Empty)
                {
                    productToEdit.PicName = picName;
                }

                if(picture != null)
                {
                    string picUrl = await _fireBaseService.UploadStorage(picture, "product_folder", productToEdit.PicName);
                    productToEdit.PicUrl = picUrl;
                }

                bool res = await _repository.Edit(productToEdit);
                if(!res)
                    throw new TaskCanceledException("No se ha podido editar el producto");

                Product editedProduct = productQuery.Include(p => p.Category).First();
                return editedProduct;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> Delete(int productId)
        {
            try
            {
                Product productFound = await _repository.Get(p => p.ProductId == productId);

                if(productFound == null)
                    throw new TaskCanceledException("El producto no existe");

                string picName = productFound.PicName;

                bool res = await _repository.Delete(productFound);

                if (res)
                    await _fireBaseService.DeleteStorage("product_folder", picName);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
