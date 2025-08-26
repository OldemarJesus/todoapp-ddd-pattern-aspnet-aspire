using System;
using FluentValidation;

namespace Web.Api.Features.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(255);

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }

}
