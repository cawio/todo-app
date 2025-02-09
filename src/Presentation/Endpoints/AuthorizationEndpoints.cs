using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Presentation.Endpoints;

public static class AuthorizationEndpoints
{
    public static WebApplication MapAuthorizationEndpoints(this WebApplication app)
    {
        app.MapGet(
                "/api/auth/login",
                async context =>
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
            )
            .AllowAnonymous();

        app.MapGet(
            "/api/auth/logout",
            async context =>
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                context.Response.Redirect("/");
            }
        );

        app.MapGet(
                "/api/auth/me",
                (HttpContext context) =>
                {
                    if (context.User?.Identity != null && context.User.Identity.IsAuthenticated)
                    {
                        return Results.Ok(
                            new
                            {
                                Username = context.User.Identity.Name,
                                Claims = context.User.Claims.Select(c => new { c.Type, c.Value })
                            }
                        );
                    }
                    return Results.Unauthorized();
                }
            )
            .RequireAuthorization();

        return app;
    }
}
