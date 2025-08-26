using FluentValidation;

namespace Web.Api.Features.Todo.Commands.CreateTodoCommand;

public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

        RuleFor(x => x.MemberId.Value)
            .NotNull()
            .WithMessage("MemberId is required.");
    }
}
