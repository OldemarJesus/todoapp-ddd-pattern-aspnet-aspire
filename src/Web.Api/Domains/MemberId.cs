namespace Web.Api.Domains;

public record MemberId(Guid Value)
{
    public static MemberId New() => new(Guid.NewGuid());
    public static MemberId FromGuid(Guid value) => new(value);
}
