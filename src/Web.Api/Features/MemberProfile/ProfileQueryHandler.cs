using SharedKernel.Abstractions;
using SharedKernel.Abstractions.Messaging.Query;
using Web.Api.Domains;
using Web.Api.Infrastructure.Repositories;

namespace Web.Api.Features.MemberProfile;

public class ProfileQueryHandler : IQueryHandler<ProfileQuery, ProfileResponse?>
{
    private readonly MemberRepository memberRepository;

    public ProfileQueryHandler(MemberRepository memberRepository)
    {
        this.memberRepository = memberRepository;
    }

    public async Task<Result<ProfileResponse?>> Handle(ProfileQuery query, CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetByIdAsync(query.MemberId, cancellationToken);
        if (member == null)
        {
            return Result<ProfileResponse?>.Success(null);
        }

        var profileResponse = new ProfileResponse(
            member.Id.Value,
            member.Username,
            member.FullName
        );

        return Result<ProfileResponse?>.Success(profileResponse);
    }
}
