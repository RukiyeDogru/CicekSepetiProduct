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

            var 

            var response = new ServiceResponse<Product>();
            if (response.Validation(new ProductValidation().Validate(product)))
            {
                response.Result = _ProductService.Add(product);
            }

            return response;










        }
    }
}
