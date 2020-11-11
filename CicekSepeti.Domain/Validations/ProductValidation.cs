 
using FluentValidation;
using CicekSepeti.Infra.Data.Entity;

namespace CicekSepeti.Domain.Validations
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public ProductValidation()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product Name cannot be empty");
            RuleFor(x => x.Piece).NotEmpty().WithMessage("You must enter the amount of product");


        }
    }
}
