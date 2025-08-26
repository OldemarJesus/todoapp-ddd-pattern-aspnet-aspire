namespace Web.Api.Domains;

public record TodoItemId(Guid Value)
{
    public static TodoItemId New() => new(Guid.NewGuid());
    public static TodoItemId FromGuid(Guid id) => new(id);
}
