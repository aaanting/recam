using System;
using FluentValidation;
using Remp.Service.DTOs;

namespace Remp.Service.Validators;

public class RegisterAgentRequestValidator : AbstractValidator<RegisterAgentRequestDto>
{
  public RegisterAgentRequestValidator()
  {
    RuleFor(x => x.Email).NotEmpty().EmailAddress();
    RuleFor(x => x.Password).NotEmpty();
    RuleFor(x => x.ConfirmPassword)
        .Equal(x => x.Password)
        .WithMessage("The password and confirmation password do not match.");
    RuleFor(x => x.AgentFirstName).NotEmpty().MaximumLength(30);
    RuleFor(x => x.AgentLastName).NotEmpty().MaximumLength(30);
  }
}
