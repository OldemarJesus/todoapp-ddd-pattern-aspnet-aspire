using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions;
using SharedKernel.Abstractions.Messaging.Command;
using Web.Api.Domains;
using Web.Api.Infrastructure;

namespace Web.Api.Features.Todo.Commands.UpdateTodoCommand;

public class UpdateTodoCommandHandler : ICommandHandler<UpdateTodoCommand, TodoItem>
{
    private readonly TodoDbContext _dbContext;

    public UpdateTodoCommandHandler(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<TodoItem>> Handle(UpdateTodoCommand command, CancellationToken cancellationToken)
    {
        var todoItem = await _dbContext.TodoItems
            .FirstOrDefaultAsync(x => x.Id == command.Id && x.MemberId == command.MemberId, cancellationToken);
        if (todoItem is null)
            return Result<TodoItem>.Failure(new Error("Todo item not found."));

        todoItem.Update(command.Title, command.Description);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result<TodoItem>.Success(todoItem);
    }
}
