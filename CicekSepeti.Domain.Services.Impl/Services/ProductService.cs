using CicekSepeti.Domain.Model.Base;
using CicekSepeti.Domain.Services.Services;
using CicekSepeti.Domain.Validations;
using CicekSepeti.Infra.Data.Entity;
using CicekSepeti.Infra.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace CicekSepeti.Domain.Services.Impl.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> ProductRepository;

        public ProductService(IRepository<Product> ProductRepository)
        {
            this.ProductRepository = ProductRepository;
        }

        public ServiceResponse<Product> Add(Product Product)
        {
            var response = new ServiceResponse<Product>();
            if (response.Validation(new ProductValidation().Validate(Product)))
            {
                response.Result = ProductRepository.Insert(Product);
            }

            return response;
        }

        public ServiceResponse<bool> Delete(int id)
        {
            var response = new ServiceResponse<bool>();
            var repoResponse = ProductRepository.GetById(id);

            response.Result = false;
            if (repoResponse != null)
            {
                ProductRepository.Delete(repoResponse);
                response.Result = true;
            }
            else
            {
                response.SetError("No Records Found");
            }
            return response;
        }

        public ServiceResponse<Product> GetProductById(int id)
        {
            var response = new ServiceResponse<Product>();

            response.Result = ProductRepository.Get(x=>x.Id==id&&!x.IsDeleted);
            if (response.Result != null)
            {

            response.IsSucceeded = true;
                response.HasError = false;

            }

            else
            {
                response.IsSucceeded = false;
                response.HasError = true;

                response.ErrorMessage = "No Records Found";
            }
            return response;
        }

        public ServiceResponse<Product> Update(Product Product)
        {

            var response = new ServiceResponse<Product>();
            if (response.Validation(new ProductValidation().Validate(Product)))
            {
                ProductRepository.Detach(Product);

                var repositoryResponse = ProductRepository.GetById(Product.Id);
                if (repositoryResponse != null)
                {
                    repositoryResponse.ProductName = Product.ProductName;
                    repositoryResponse.Piece = Product.Piece;
                    repositoryResponse.IsActive = Product.IsActive;
                    repositoryResponse.IsDeleted = Product.IsDeleted;
                    response.Result = ProductRepository.Update(repositoryResponse);
                }
                else
                {
                    response.SetError("Veri Bulunamadı");
                }

            }

            return response;


        }
    }
}
