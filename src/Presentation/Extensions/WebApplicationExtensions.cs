using Presentation.Endpoints;

namespace Presentation.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        #region API Configuration

        _ = app.UseCors("Frontend");
        _ = app.UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        _ = app.UseHsts();
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        #endregion API Configuration

        #region MinimalApi

        app.MapAuthorizationEndpoints();
        app.MapTodoListEndpoints();

        #endregion MinimalApi

        #region Custom Middleware

        app.UseMiddleware<SetCurrentUserMiddleware>();

        #endregion Custom Middleware

        return app;
    }
}
