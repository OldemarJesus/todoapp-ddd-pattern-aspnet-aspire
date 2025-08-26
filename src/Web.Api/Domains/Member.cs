namespace Web.Api.Domains;

public class Member
{
    private readonly HashSet<TodoItem> todoItems = new();
    public MemberId Id { get; private set; } = null!;
    public string Username { get; private set; } = null!;

    public string Password { get; private set; } = null!;

    public string FullName { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<TodoItem> TodoItems => todoItems.ToList().AsReadOnly();

    private Member()
    {

    }

    public static Member Create(MemberId id, string username, string hashedPassword, string fullName)
    {
        return new Member
        {
            Id = id,
            Username = username,
            Password = hashedPassword,
            FullName = fullName,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Member Create(string username, string hashedPassword, string fullName)
    {
        return new Member
        {
            Id = MemberId.New(),
            Username = username,
            Password = hashedPassword,
            FullName = fullName,
            CreatedAt = DateTime.UtcNow
        };
    }
}
