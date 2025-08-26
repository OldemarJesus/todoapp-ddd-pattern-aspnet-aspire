using SharedKernel.Abstractions.Messaging.Command;

namespace Web.Api.Features.Register;

public record RegisterCommand(string Username, string Password, string FullName) : ICommand<RegisterResponse>;