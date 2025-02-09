using System.Security.Claims;
using Application.Services;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Presentation.Endpoints;

public static class AuthorizationEndpoints
{
    public static WebApplication MapAuthorizationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("auth")
            .WithDescription("Endpoints for user authentication")
            .WithOpenApi();

        _ = group.MapGet("/login", Login).WithSummary("Login");
        _ = group.MapGet("/logout", Logout).WithSummary("Logout").RequireAuthorization();
        _ = group
            .MapGet("/me", Me)
            .WithSummary("Get currently logged in User")
            .RequireAuthorization();

        return app;
    }

    private static async void Login(HttpContext context)
    {
        if (context.User?.Identity == null || !context.User.Identity.IsAuthenticated)
        {
            await context.ChallengeAsync(
                DiscordAuthenticationDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = "https://localhost:4200/" }
            );
        }
        else
        {
            context.Response.Redirect("https://localhost:4200/");
        }
    }

    private static async void Logout(HttpContext context)
    {
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        context.Response.Redirect("/");
    }

    private static async Task<IResult> Me(IUserService userService, HttpContext context)
    {
        if (context.User?.Identity != null && context.User.Identity.IsAuthenticated)
        {
            var discordId = context
                .User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (discordId == null)
            {
                return Results.Unauthorized();
            }

            var user = await userService.GetUserByDiscordIdAsync(ulong.Parse(discordId));

            if (user == null)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(user);
        }

        return Results.Unauthorized();
    }
}
