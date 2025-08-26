using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions;
using SharedKernel.Abstractions.Messaging.Query;
using Web.Api.Domains;
using Web.Api.Infrastructure;

namespace Web.Api.Features.Todo.Queries.GetTodoQuery;

public class GetTodoQueryHandler : IQueryHandler<GetTodoQuery, TodoItem>
{
    private readonly TodoDbContext _dbContext;

    public GetTodoQueryHandler(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<TodoItem>> Handle(GetTodoQuery query, CancellationToken cancellationToken)
    {
        var todo = await _dbContext.TodoItems
            .FirstOrDefaultAsync(x => x.Id == query.TodoItemId && x.MemberId == query.MemberId, cancellationToken);

        return todo is not null
            ? Result<TodoItem>.Success(todo)
            : Result<TodoItem>.Failure(new Error("Todo not found"));
    }
}
