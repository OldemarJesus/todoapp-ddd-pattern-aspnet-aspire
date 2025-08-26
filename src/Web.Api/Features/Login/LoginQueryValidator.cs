using FluentValidation;

namespace Web.Api.Features.Login;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
