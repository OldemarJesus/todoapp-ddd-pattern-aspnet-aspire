using FluentValidation;

namespace Web.Api.Features.Todo.Commands.UpdateTodoCommand;

public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
    public UpdateTodoCommandValidator()
    {
        RuleFor(x => x.Id.Value)
            .NotNull()
            .WithMessage("Id is required.");

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
