using FluentValidation;
using Remp.Service.DTOs;

namespace Remp.Service.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterRequestDto>
{
  public RegisterUserRequestValidator()
  {
    RuleFor(x => x.Email).NotEmpty().EmailAddress();
    RuleFor(x => x.PhotographyCompanyName).NotEmpty().MaximumLength(60);
    RuleFor(x => x.Password).NotEmpty();
    RuleFor(x => x.ConfirmPassword)
        .Equal(x => x.Password)
        .WithMessage("The password and confirmation password do not match.");
  }
}
