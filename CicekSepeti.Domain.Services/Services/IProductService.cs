using CicekSepeti.Domain.Model.Base;
using CicekSepeti.Infra.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CicekSepeti.Domain.Services.Services
{
   public interface IProductService
    {
        ServiceResponse<Product> Add(Product Product);
        ServiceResponse<Product> Update(Product Product);
        ServiceResponse<bool> Delete(int id);
        ServiceResponse<Product> GetProductById(int id);
    }
}
