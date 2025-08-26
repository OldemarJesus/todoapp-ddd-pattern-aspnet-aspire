using SharedKernel.Abstractions.Messaging.Command;
using Web.Api.Domains;

namespace Web.Api.Features.Todo.Commands.UpdateTodoCommand;

public record UpdateTodoCommand(TodoItemId Id, string Title, string Description, MemberId MemberId) : ICommand<TodoItem>;