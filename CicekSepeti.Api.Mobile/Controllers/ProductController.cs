using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CicekSepeti.Api.Mobile.Model;
using CicekSepeti.Domain.Services.Services;
using CicekSepeti.Infra.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CicekSepeti.Api.Mobile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _ProductService;

        public ProductController(IProductService productService)
        {
            _ProductService = productService;
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200, "", typeof(Product))]
        public IActionResult Get(int id)
        {
            var ProductResponse = _ProductService.GetProductById(id);

            if (ProductResponse.IsSucceeded && ProductResponse.Result != null)
            {
                return Ok(ProductResponse.Result);
            }


            return ProductResponse.HttpGetResponse();
        }


        [HttpPost("add-Product")]
        [SwaggerResponse(200, "", typeof(Product))]
        public IActionResult Add( ProductViewModel model)
        {
            var ProductResponse = _ProductService.Add(new Product
            {
                ProductName = model.ProductName,
                Price=model.Price,
                Piece=model.Piece
               
            }) ;

            if (ProductResponse.IsSucceeded && ProductResponse.Result != null)
            {

                return Ok(ProductResponse.Result);

            }
            return ProductResponse.HttpGetResponse();

        }

        [HttpPost("delete/{id}")]
        [SwaggerResponse(200, "", typeof(Product))]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var getProduct = _ProductService.GetProductById(id);

                if (getProduct != null && getProduct.IsSucceeded)
                {
                    
                    getProduct.Result.IsDeleted = true;
                    getProduct.Result.IsActive = false;

                    var updateResult = _ProductService.Update(getProduct.Result);

                    if (updateResult.IsSucceeded)
                    {
                        return Ok(updateResult.Result);
                    }
                    return updateResult.HttpGetResponse();
                }
                return getProduct.HttpGetResponse();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost("edit-Product/{id}")]
        [SwaggerResponse(200, "", typeof(Product))]
        public IActionResult Edit(int id, ProductViewModel model)
        {
            var getProduct = _ProductService.GetProductById(id);

            if (getProduct != null && getProduct.IsSucceeded)
            {
                getProduct.Result.ProductName = model.ProductName;
                getProduct.Result.Piece = model.Piece;
                getProduct.Result.Price = model.Price;


                var updateResult = _ProductService.Update(getProduct.Result);

                if (updateResult.IsSucceeded)
                {
                    return Ok(updateResult.Result);
                }
                return updateResult.HttpGetResponse();
            }
            return getProduct.HttpGetResponse();
        }

    }
}
