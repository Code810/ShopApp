using FluentValidation;
using ShopApp.Apps.AdminApp.Dtos.UserDto;

namespace ShopApp.Apps.AdminApp.Validators.UserValidators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(r => r.Email).NotEmpty().EmailAddress();
            RuleFor(r => r.FullName).NotEmpty().MaximumLength(30);
            RuleFor(r => r.UserName).NotEmpty().MaximumLength(30);
            RuleFor(r => r.Password).NotEmpty().MinimumLength(8).MaximumLength(15);
            RuleFor(r => r.RePassword).NotEmpty().MinimumLength(8).MaximumLength(15);
            RuleFor(r => r).Custom((r, context) =>
            {
                if (r.Password != r.RePassword)
                {
                    context.AddFailure("Password", "Password and Repassword must be the same ");
                }

            });
        }
    }
}
