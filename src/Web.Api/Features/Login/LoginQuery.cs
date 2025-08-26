using SharedKernel.Abstractions.Messaging.Query;
using Web.Api.Domains;

namespace Web.Api.Features.Login;

public record LoginQuery(string Username, string Password) : IQuery<LoginResponse>;
