using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application;

public class TodoListService(ApplicationDbContext dbContext) : ITodoListService
{
    ApplicationDbContext _dbContext = dbContext;

    public Task<List<TodoList>> GetTodoListsAsync()
    {
        return _dbContext
            .TodoLists.Include(tl => tl.Tasks)
            .ThenInclude(t => t.TaskTags)
            .ToListAsync();
    }

    public async Task<TodoList> GetTodoListAsync(int id)
    {
        var todoList =
            await _dbContext.TodoLists.FindAsync(id)
            ?? throw new KeyNotFoundException("Todo list not found");

        return todoList;
    }

    public Task<TodoList> CreateTodoListAsync(TodoList todoList)
    {
        _dbContext.TodoLists.Add(todoList);

        return _dbContext.SaveChangesAsync().ContinueWith(_ => todoList);
    }

    public Task<TodoList> UpdateTodoListAsync(int todoListId, TodoList todoList)
    {
        if (todoList.Id != todoListId)
        {
            throw new ArgumentException("Todo list ID does not match");
        }

        todoList.UpdatedAt = DateTimeOffset.UtcNow;
        _dbContext.TodoLists.Update(todoList);

        return _dbContext.SaveChangesAsync().ContinueWith(_ => todoList);
    }

    public System.Threading.Tasks.Task DeleteTodoListAsync(int id)
    {
        var todoListToDelete =
            _dbContext.TodoLists.Find(id) ?? throw new KeyNotFoundException("Todo list not found");

        _dbContext.TodoLists.Remove(todoListToDelete);

        return _dbContext.SaveChangesAsync();
    }
}
