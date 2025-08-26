using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging.Query;
using Web.Api.Domains;

namespace Web.Api.Features.MemberProfile;

public static class ProfileEndpoint
{
    public static void MapProfileEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/profile", async ([FromServices] IQueryHandler<ProfileQuery, ProfileResponse?> handler,
            [FromServices] IHttpContextAccessor httpContextAccessor,
            CancellationToken cancellationToken) =>
        {
            var memberIdStr = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var memberId = new MemberId(Guid.Parse(memberIdStr));

            var query = new ProfileQuery(memberId);
            var result = await handler.Handle(query, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        }).RequireAuthorization();
    }
}
