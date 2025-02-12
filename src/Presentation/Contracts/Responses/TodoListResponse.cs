using Infrastructure.Entities;
using Task = Infrastructure.Entities.Task;

namespace Presentation.Contracts.Responses;

public class TodoListResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<TaskResponse> Tasks { get; set; } = [];

    public static TodoListResponse FromEntity(TodoList entity)
    {
        var result = new TodoListResponse()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Title = entity.Title,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };

        foreach (var task in entity.Tasks)
        {
            result.Tasks.Add(TaskResponse.FromEntity(task));
        }

        return result;
    }

    public TodoList ToEntity()
    {
        var result = new TodoList()
        {
            Id = Id,
            UserId = UserId,
            Title = Title,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
        };

        foreach (var task in Tasks)
        {
            result.Tasks.Add(task.ToEntity());
        }

        return result;
    }
}
