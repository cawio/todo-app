namespace Infrastructure.Entities;

public class User
{
    public int Id { get; set; }
    public ulong DiscordId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProfilePictureId { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<TodoList> TodoLists { get; set; } = [];
}
