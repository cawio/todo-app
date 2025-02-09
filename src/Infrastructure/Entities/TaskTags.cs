namespace Infrastructure.Entities;

public class TaskTags
{
    public int TaskId { get; set; }
    public Task Task { get; set; } = null!;
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
