using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application;

public class TodoListService(ApplicationDbContext dbContext) : ITodoListService
{
    ApplicationDbContext _dbContext = dbContext;

    public Task<List<TodoList>> GetTodoListsAsync(int userId)
    {
        return _dbContext
            .TodoLists.Include(tl => tl.Tasks)
            .ThenInclude(t => t.TaskTags)
            .Where(tl => tl.UserId == userId)
            .ToListAsync();
    }

    public async Task<TodoList> GetTodoListAsync(int id, int userId)
    {
        var todoList =
            await _dbContext.TodoLists.FindAsync(id)
            ?? throw new KeyNotFoundException("Todo list not found");

        if (!IsUserAllowedToAccessTodoList(userId, todoList))
        {
            throw new UnauthorizedAccessException("User is not allowed to access this todo list");
        }

        return todoList;
    }

    public Task<TodoList> CreateTodoListAsync(int userId, TodoList todoList)
    {
        _dbContext.TodoLists.Add(todoList);

        return _dbContext.SaveChangesAsync().ContinueWith(_ => todoList);
    }

    public Task<TodoList> UpdateTodoListAsync(int todoListId, int userId, TodoList todoList)
    {
        if (todoList.Id != todoListId)
        {
            throw new ArgumentException("Todo list ID does not match");
        }

        if (!IsUserAllowedToAccessTodoList(userId, todoList))
        {
            throw new UnauthorizedAccessException("User is not allowed to update this todo list");
        }

        todoList.UpdatedAt = DateTimeOffset.UtcNow;
        _dbContext.TodoLists.Update(todoList);

        return _dbContext.SaveChangesAsync().ContinueWith(_ => todoList);
    }

    public System.Threading.Tasks.Task DeleteTodoListAsync(int id, int userId)
    {
        var todoListToDelete =
            _dbContext.TodoLists.Find(id) ?? throw new KeyNotFoundException("Todo list not found");

        if (!IsUserAllowedToAccessTodoList(userId, todoListToDelete))
        {
            throw new UnauthorizedAccessException("User is not allowed to delete this todo list");
        }

        _dbContext.TodoLists.Remove(todoListToDelete);

        return _dbContext.SaveChangesAsync();
    }

    private static bool IsUserAllowedToAccessTodoList(int userId, TodoList todoList)
    {
        return todoList.UserId == userId;
    }
}
