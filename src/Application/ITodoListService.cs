using Infrastructure.Entities;

namespace Application;

public interface ITodoListService
{
    Task<List<TodoList>> GetTodoListsAsync(int userId);
    Task<TodoList> GetTodoListAsync(int id, int userId);
    Task<TodoList> CreateTodoListAsync(int userId, TodoList todoList);
    Task<TodoList> UpdateTodoListAsync(int todoListId, int userId, TodoList todoList);
    System.Threading.Tasks.Task DeleteTodoListAsync(int id, int userId);
}
