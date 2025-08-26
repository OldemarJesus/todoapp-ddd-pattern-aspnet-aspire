using SharedKernel.Abstractions.Messaging.Command;
using Web.Api.Domains;

namespace Web.Api.Features.Todo.Commands.CreateTodoCommand;

    public record CreateTodoCommand(string Title, string Description, MemberId MemberId) : ICommand<TodoItemId>;