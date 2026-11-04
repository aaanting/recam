using FluentValidation;
using Remp.Service.DTOs;

namespace Remp.Service.Validators;

public class CreateAgentRequestValidator : AbstractValidator<CreateAgentRequestDto>
{
    public CreateAgentRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.AgentFirstName).NotEmpty().MaximumLength(30);
        RuleFor(x => x.AgentLastName).NotEmpty().MaximumLength(30);
    }
}
