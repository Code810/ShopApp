using FluentValidation;
using ShopApp.Apps.AdminApp.Dtos.ProductDto;

namespace ShopApp.Apps.AdminApp.Validators.ProductValidator
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Not empty...")
                .MaximumLength(50).WithMessage("Name maxsimum lenght limit is 50");
            RuleFor(p => p.SalePrice).NotEmpty().WithMessage("Not empty...");
            RuleFor(p => p.CostPrice).NotEmpty().WithMessage("Not empty...");
            RuleFor(p => p).Custom((p, context) =>
                {
                    if (p.SalePrice < p.CostPrice)
                    {
                        context.AddFailure("SalePrice", "Sale price can not be less than Cost Price");
                    }
                });
        }
    }
}
