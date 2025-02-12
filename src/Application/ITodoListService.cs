using Infrastructure.Entities;

namespace Application;

public interface ITodoListService
{
    Task<List<TodoList>> GetTodoListsAsync();
    Task<TodoList> GetTodoListAsync(int id);
    Task<TodoList> CreateTodoListAsync(TodoList todoList);
    Task<TodoList> UpdateTodoListAsync(int todoListId, TodoList todoList);
    System.Threading.Tasks.Task DeleteTodoListAsync(int id);
}
