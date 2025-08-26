using SharedKernel.Abstractions.Messaging.Command;
using Web.Api.Domains;

namespace Web.Api.Features.Todo.Commands.DeleteTodoCommand;

public record DeleteTodoCommand(TodoItemId Id, MemberId MemberId) : ICommand;
