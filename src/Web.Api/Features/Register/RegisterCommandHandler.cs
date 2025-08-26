using FluentValidation;
using SharedKernel.Abstractions;
using SharedKernel.Abstractions.Messaging.Command;
using Web.Api.Domains;
using Web.Api.Infrastructure;
using Web.Api.Infrastructure.Abstrations;
using Web.Api.Infrastructure.Repositories;
using Web.Api.Services;

namespace Web.Api.Features.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterResponse>
{
    private readonly IValidator<RegisterCommand> _validator;
    private readonly MemberRepository _memberRepository;
    private readonly PasswordHasher _passwordHasher;
    private readonly TodoDbContext _dbContext;
    private readonly TokenProvider _tokenService;

    public RegisterCommandHandler(IValidator<RegisterCommand> validator, MemberRepository memberRepository, PasswordHasher passwordHasher, TodoDbContext dbContext, TokenProvider tokenService)
    {
        _validator = validator;
        _memberRepository = memberRepository;
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
        _tokenService = tokenService;
    }
    public async Task<Result<RegisterResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<RegisterResponse>.Failure(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        // verify if user already exists
        var userExists = await _memberRepository.ExistsAsync(command.Username, cancellationToken);
        if (userExists)
        {
            return Result<RegisterResponse>.Failure(new Error("Username already exists."));
        }

        // create new user
        var hashPassword = _passwordHasher.Hash(command.Password);
        var user = Member.Create(command.Username, hashPassword, command.FullName);
        await _dbContext.Members.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var token = _tokenService.Create(user);
        return Result<RegisterResponse>.Success(new RegisterResponse(token));
    }
}
