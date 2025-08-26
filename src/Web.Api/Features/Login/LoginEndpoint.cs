using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging.Query;
using Web.Api.Domains;

namespace Web.Api.Features.Login;

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLoginEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/login", async ([FromBody] LoginRequest request, [FromServices] IQueryHandler<LoginQuery, LoginResponse> queryHandler, CancellationToken cancellationToken) =>
        {
            var query = new LoginQuery(request.Username, request.Password);
            var result = await queryHandler.Handle(query, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        });

        return builder;
    }
}
