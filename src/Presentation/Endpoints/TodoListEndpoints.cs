using System.Security.Claims;
using Application;
using Infrastructure.Entities;

namespace Presentation.Endpoints;

public static class TodoListEndpoints
{
    public static void MapTodoListEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/todo-lists")
            .WithTags("todo-lists")
            .WithDescription("Endpoints for managing todo lists")
            .WithOpenApi();

        _ = group
            .MapGet("", GetTodoLists)
            .Produces<List<TodoList>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup all TodoLists")
            .WithDescription("\n    GET /api/todo-lists\n    ")
            .RequireAuthorization();

        _ = group
            .MapGet("/{id}", GetTodoList)
            .Produces<TodoList>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Lookup a TodoList by ID")
            .WithDescription("\n    GET /api/todo-lists/{id}\n    ")
            .RequireAuthorization();

        _ = group
            .MapPost("", CreateTodoList)
            .Accepts<TodoList>("application/json")
            .Produces<TodoList>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new TodoList")
            .WithDescription("\n    POST /api/todo-lists\n    ")
            .RequireAuthorization();

        _ = group
            .MapPut("/{id}", UpdateTodoList)
            .Accepts<TodoList>("application/json")
            .Produces<TodoList>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update a TodoList by ID")
            .WithDescription("\n    PUT /api/todo-lists/{id}\n    ")
            .RequireAuthorization();

        _ = group
            .MapDelete("/{id}", DeleteTodoList)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a TodoList by ID")
            .WithDescription("\n    DELETE /api/todo-lists/{id}\n    ")
            .RequireAuthorization();
    }

    private static async Task<IResult> GetTodoLists(
        ITodoListService todoListService,
        HttpContext context
    )
    {
        var userId = context
            .User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;
        var todoLists = await todoListService.GetTodoListsAsync(int.Parse(userId!));
        return Results.Ok(todoLists);
    }

    private static async Task<IResult> GetTodoList(
        ITodoListService todoListService,
        HttpContext context,
        int id
    )
    {
        var userId = context
            .User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;
        var todoList = await todoListService.GetTodoListAsync(id, int.Parse(userId!));
        return Results.Ok(todoList);
    }

    private static async Task<IResult> CreateTodoList(
        ITodoListService todoListService,
        HttpContext context,
        TodoList todoList
    )
    {
        var userId = context
            .User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;
        var newTodoList = await todoListService.CreateTodoListAsync(int.Parse(userId!), todoList);
        return Results.Created($"/api/todo-lists/{newTodoList.Id}", newTodoList);
    }

    private static async Task<IResult> UpdateTodoList(
        ITodoListService todoListService,
        HttpContext context,
        int id,
        TodoList todoList
    )
    {
        var userId = context
            .User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;
        var updatedTodoList = await todoListService.UpdateTodoListAsync(
            id,
            int.Parse(userId!),
            todoList
        );
        return Results.Ok(updatedTodoList);
    }

    private static async Task<IResult> DeleteTodoList(
        ITodoListService todoListService,
        HttpContext context,
        int id
    )
    {
        var userId = context
            .User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;
        await todoListService.DeleteTodoListAsync(id, int.Parse(userId!));
        return Results.NoContent();
    }
}
