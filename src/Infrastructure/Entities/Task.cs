namespace Infrastructure.Entities;

public class Task
{
    public int Id { get; set; }
    public int TodoListId { get; set; }
    public TodoList TodoList { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTimeOffset DueOn { get; set; }
    public int Priority { get; set; }
    public bool Completed { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<TaskTags> TaskTags { get; set; } = [];
}
