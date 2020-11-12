using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CicekSepeti.Api.Mobile.Model;
using CicekSepeti.Domain.Model.Base;
using CicekSepeti.Domain.Services.Services;
using CicekSepeti.Domain.Validations;
using CicekSepeti.Infra.Data.Entity;
using CicekSepeti.Infra.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CicekSepeti.Api.Mobile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private IProductService _ProductService;

        public CardController(IProductService productService)
        {
            _ProductService = productService;

        }


        [HttpPost("add")]
        public IActionResult Add(CardViewModel model)
        {

            var ProductResponse = _ProductService.GetProductById(model.ProductId);

            if (ProductResponse.IsSucceeded && ProductResponse.Result != null)
            {

                if(ProductResponse.Result.Piece>=model.Quantity)
                {
                    ProductResponse.Result.Piece = ProductResponse.Result.Piece - model.Quantity;

                    _ProductService.Update(ProductResponse.Result);
                    return Ok(new
                    {
                        success = true,
                        QuantityCount = ProductResponse.Result.Piece


                    }) ;


                }
                else
                {
                    return Ok(new
                    {
                        success = false,
                        QuantityCount = ProductResponse.Result.Piece


                    });

                }

            }
            return ProductResponse.HttpGetResponse();


        }
    }
}
