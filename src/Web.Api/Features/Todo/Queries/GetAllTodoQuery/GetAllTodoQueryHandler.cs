using Microsoft.EntityFrameworkCore;
using Web.Api.Domains;
using Web.Api.Infrastructure;
using SharedKernel.Abstractions;
using SharedKernel.Abstractions.Messaging.Query;

namespace Web.Api.Features.Todo.Queries.GetAllTodoQuery;

public class GetAllTodoQueryHandler : IQueryHandler<GetAllTodoQuery, IEnumerable<TodoItem>>
{
    private readonly TodoDbContext _dbContext;

    public GetAllTodoQueryHandler(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<IEnumerable<TodoItem>>> Handle(GetAllTodoQuery query, CancellationToken cancellationToken)
    {
        var todos = await _dbContext.TodoItems.
            Where(x => x.MemberId == query.MemberId)
            .ToListAsync(cancellationToken);
        return Result<IEnumerable<TodoItem>>.Success(todos);
    }
}
