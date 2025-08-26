using SharedKernel.Abstractions.Messaging.Query;
using Web.Api.Domains;

namespace Web.Api.Features.Todo.Queries.GetAllTodoQuery;

public record GetAllTodoQuery(MemberId MemberId) : IQuery<IEnumerable<TodoItem>>;
