using SharedKernel.Abstractions.Messaging.Query;
using Web.Api.Domains;

namespace Web.Api.Features.Todo.Queries.GetTodoQuery;

public record GetTodoQuery(TodoItemId TodoItemId, MemberId MemberId) : IQuery<TodoItem>;
