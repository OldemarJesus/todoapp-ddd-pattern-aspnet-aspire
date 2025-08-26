using FluentValidation;
using SharedKernel.Abstractions;
using SharedKernel.Abstractions.Messaging.Query;
using Web.Api.Domains;
using Web.Api.Infrastructure.Abstrations;
using Web.Api.Infrastructure.Repositories;
using Web.Api.Services;

namespace Web.Api.Features.Login;

internal sealed class LoginQueryHandler
    : IQueryHandler<LoginQuery, LoginResponse>
{
    private readonly IValidator<LoginQuery> _validator;
    private readonly MemberRepository _memberRepository;
    private readonly PasswordHasher _passwordHasher;
    private readonly TokenProvider _tokenProvider;

    public LoginQueryHandler(IValidator<LoginQuery> validator, MemberRepository memberRepository, PasswordHasher passwordHasher, TokenProvider tokenProvider)
    {
        _validator = validator;
        _memberRepository = memberRepository;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<LoginResponse>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<LoginResponse>.Failure(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        var member = await _memberRepository.GetByUsernameAsync(query.Username, cancellationToken);
        if (member is null || !_passwordHasher.Verify(query.Password, member.Password))
        {
            return Result<LoginResponse>.Failure(new Error("Invalid credentials."));
        }

        string token = _tokenProvider.Create(member);
        return Result<LoginResponse>.Success(new LoginResponse(token));
    }
}
