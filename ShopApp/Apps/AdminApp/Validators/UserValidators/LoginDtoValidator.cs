using FluentValidation;
using ShopApp.Apps.AdminApp.Dtos.UserDto;

namespace ShopApp.Apps.AdminApp.Validators.UserValidators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(r => r.UserNameOrEmail).NotEmpty().MaximumLength(30);
            RuleFor(r => r.Password).NotEmpty().MinimumLength(8).MaximumLength(15);
        }
    }
}
