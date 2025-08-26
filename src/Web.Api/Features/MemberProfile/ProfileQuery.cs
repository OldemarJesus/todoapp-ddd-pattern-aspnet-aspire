using SharedKernel.Abstractions.Messaging.Query;
using Web.Api.Domains;

namespace Web.Api.Features.MemberProfile;

public record ProfileQuery(MemberId MemberId) : IQuery<ProfileResponse?>;