namespace Presentation.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        #region Security

        _ = app.UseHsts();

        #endregion Security

        #region API Configuration

        _ = app.UseCors();
        _ = app.UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        #endregion API Configuration


        #region MinimalApi

        #endregion MinimalApi

        return app;
    }
}
