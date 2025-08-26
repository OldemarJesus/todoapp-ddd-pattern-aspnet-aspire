using SharedKernel.Abstractions;
using SharedKernel.Abstractions.Messaging.Command;
using Web.Api.Domains;
using Web.Api.Infrastructure;

namespace Web.Api.Features.Todo.Commands.CreateTodoCommand;

public class CreateTodoCommandHandler : ICommandHandler<CreateTodoCommand, TodoItemId>
{
    private readonly TodoDbContext _dbContext;

    public CreateTodoCommandHandler(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<TodoItemId>> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        var todoItem = TodoItem.Create(command.Title, command.Description, command.MemberId);
        await _dbContext.TodoItems.AddAsync(todoItem, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result<TodoItemId>.Success(todoItem.Id);
    }
}
