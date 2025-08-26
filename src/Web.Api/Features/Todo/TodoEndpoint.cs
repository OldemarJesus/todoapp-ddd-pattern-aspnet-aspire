using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging.Command;
using SharedKernel.Abstractions.Messaging.Query;
using Web.Api.Domains;
using Web.Api.Features.Todo.Commands.CreateTodoCommand;
using Web.Api.Features.Todo.Commands.DeleteTodoCommand;
using Web.Api.Features.Todo.Commands.UpdateTodoCommand;
using Web.Api.Features.Todo.Queries.GetAllTodoQuery;
using Web.Api.Features.Todo.Queries.GetTodoQuery;

namespace Web.Api.Features.Todo;

public static class TodoEndpoint
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/todos", async ([FromServices] IQueryHandler<GetAllTodoQuery, IEnumerable<TodoItem>> queryHandler,
            [FromServices] IHttpContextAccessor context,
            CancellationToken cancellationToken) =>
        {
            var memberIdStr = context.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var memberId = new MemberId(Guid.Parse(memberIdStr));

            var result = await queryHandler.Handle(new GetAllTodoQuery(memberId), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        });

        routes.MapGet("/todos/{id}", async ([FromRoute]Guid id,
            [FromServices] IQueryHandler<GetTodoQuery, TodoItem> queryHandler,
            [FromServices] IHttpContextAccessor context,
            CancellationToken cancellationToken) =>
        {
            var memberIdStr = context.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var memberId = new MemberId(Guid.Parse(memberIdStr));

            var result = await queryHandler.Handle(new GetTodoQuery(TodoItemId.FromGuid(id), memberId), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        });

        routes.MapPost("/todos", async ([FromBody] CreateTodoRequest request,
            [FromServices] ICommandHandler<CreateTodoCommand, TodoItemId> handler,
            [FromServices] IHttpContextAccessor context,
            CancellationToken cancellationToken) =>
        {
            // get id of authenticated user
            var memberIdStr = context.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var memberId = new MemberId(Guid.Parse(memberIdStr));

            var command = new CreateTodoCommand(request.Title, request.Description, memberId);
            var todoId = await handler.Handle(command, cancellationToken);
            return todoId.IsSuccess ? Results.Ok(todoId.Value) : Results.BadRequest(todoId.Error);
        }).RequireAuthorization();

        routes.MapPut("/todos/{id}", async ([FromRoute] Guid id, [FromBody] UpdateTodoRequest request,
            [FromServices] ICommandHandler<UpdateTodoCommand, TodoItem> handler,
            [FromServices] IHttpContextAccessor context,
            CancellationToken cancellationToken) =>
        {
            // get id of authenticated user
            var memberIdStr = context.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var memberId = new MemberId(Guid.Parse(memberIdStr));

            var command = new UpdateTodoCommand(TodoItemId.FromGuid(id), request.Title, request.Description, memberId);
            var result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
        });

        routes.MapDelete("/todos/{id}", async ([FromRoute] Guid id,
            [FromServices] ICommandHandler<DeleteTodoCommand> handler,
            [FromServices] IHttpContextAccessor context,
            CancellationToken cancellationToken) =>
        {
            // get id of authenticated user
            var memberIdStr = context.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var memberId = new MemberId(Guid.Parse(memberIdStr));

            var command = new DeleteTodoCommand(TodoItemId.FromGuid(id), memberId);
            var result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
        });
    }
}
