using FluentValidation;
using ShopApp.Apps.AdminApp.Dtos.CategoryDto;

namespace ShopApp.Apps.AdminApp.Validators.CategoryValidator
{
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Not empty...")
                .MaximumLength(50).WithMessage("Name maxsimum lenght limit is 50");
            RuleFor(c => c).Custom((c, context) =>
            {
                if ((c.Photo != null & c.Photo.ContentType.Contains("image/")))
                {
                    context.AddFailure("Photo", "Image type not allowed");
                }
                if (c.Photo != null && c.Photo.Length / 1024 > 500)
                {
                    context.AddFailure("Photo", "Image size is not correct");

                }
            });
        }
    }
}
