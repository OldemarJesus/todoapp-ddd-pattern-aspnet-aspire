using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions;
using SharedKernel.Abstractions.Messaging.Command;
using Web.Api.Infrastructure;

namespace Web.Api.Features.Todo.Commands.DeleteTodoCommand;

public class DeleteTodoCommandHandler : ICommandHandler<DeleteTodoCommand>
{
    private readonly TodoDbContext _dbContext;

    public DeleteTodoCommandHandler(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
    {
        var todoItem = await _dbContext.TodoItems
            .FirstOrDefaultAsync(x => x.Id == command.Id && x.MemberId == command.MemberId, cancellationToken);

        if (todoItem == null)
        {
            return Result.Failure(new Error("Todo item not found."));
        }

        _dbContext.TodoItems.Remove(todoItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
