using SharedKernel.Abstractions.Messaging.Command;
using Web.Api.Domains;

namespace Web.Api.Features.Register;

public static class RegisterEndpoint
{
    public static void MapRegisterEndpoint(this IEndpointRouteBuilder routes)
    {
        _ = routes.MapPost("/register", async (RegisterRequest request, ICommandHandler<RegisterCommand, RegisterResponse> handler, CancellationToken cancellationToken) =>
        {
            var command = new RegisterCommand(request.Username, request.Password, request.FullName);
            var result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.Created($"/members/{result.Value}", result.Value) : Results.BadRequest(result.Error);
        });
    }
}
