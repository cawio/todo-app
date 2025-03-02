namespace Infrastructure.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<TaskTags> TaskTags { get; set; } = [];
}
