namespace Web.Api.Domains;

public class TodoItem
{
    public TodoItemId Id { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public bool IsCompleted { get; private set; }
    public MemberId MemberId { get; private set; } = null!;

    public static TodoItem Create(TodoItemId id, string title, string description, MemberId memberId)
    {
        return new TodoItem
        {
            Id = id,
            Title = title,
            Description = description,
            IsCompleted = false,
            MemberId = memberId
        };
    }

    public static TodoItem Create(string title, string description, MemberId memberId)
    {
        return Create(TodoItemId.New(), title, description, memberId);
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
    }

    public void Update(string title, string description)
    {
        Title = title;
        Description = description;
    }
}
