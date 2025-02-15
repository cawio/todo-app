using Infrastructure.Entities;
using Task = Infrastructure.Entities.Task;

namespace Presentation.Contracts.Responses;

public class TaskResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTimeOffset? DueOn { get; set; }
    public int Priority { get; set; }
    public bool Completed { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<TagResponse> Tags { get; set; } = [];

    public static TaskResponse FromEntity(Task entity)
    {
        var result = new TaskResponse()
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            DueOn = entity.DueOn,
            Priority = entity.Priority,
            Completed = entity.Completed,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };

        foreach (var taskTag in entity.TaskTags)
        {
            result.Tags.Add(TagResponse.FromEntity(taskTag.Tag));
        }

        return result;
    }

    public Task ToEntity()
    {
        var result = new Task()
        {
            Id = Id,
            Title = Title,
            Description = Description,
            DueOn = DueOn,
            Priority = Priority,
            Completed = Completed,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
        };

        foreach (var tag in Tags)
        {
            result.TaskTags.Add(new TaskTags() { TaskId = Id, TagId = tag.Id });
        }

        return result;
    }
}
