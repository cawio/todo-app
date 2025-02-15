using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        _ = services.AddScoped<IUserService, UserService>();
        _ = services.AddScoped<ITodoListService, TodoListService>();

        return services;
    }
}
