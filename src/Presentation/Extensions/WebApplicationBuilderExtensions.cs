using System.Text.Json;
using System.Text.Json.Serialization;
using Application;
using Application.Services;
using AspNet.Security.OAuth.Discord;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Json;

namespace Presentation.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureApplicationBuilder(
        this WebApplicationBuilder builder
    )
    {
        #region Logging

        _ = builder.Logging.ClearProviders();
        _ = builder.Logging.AddConsole();

        #endregion Logging

        #region Serialization

        _ = builder.Services.Configure<JsonOptions>(opt =>
        {
            opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            opt.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opt.SerializerOptions.PropertyNameCaseInsensitive = true;
            opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opt.SerializerOptions.Converters.Add(
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            );
        });

        #endregion Serialization

        #region CORS

        _ = builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "Frontend",
                builder =>
                    builder
                        .WithOrigins("http://localhost:4200") // Your Angular app’s URL
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
            );
        });

        #endregion CORS

        #region OpenAPI

        _ = builder.Services.AddOpenApi();

        #endregion OpenAPI

        #region Error Handling

        _ = builder.Services.AddProblemDetails();

        #endregion Error Handling

        #region Validation

        // TODO: research how to add validation

        #endregion Validation

        #region Project Dependencies

        _ = builder.Services.AddInfrastructure(builder.Configuration);
        _ = builder.Services.AddApplication();

        #endregion Project Dependencies

        #region Authentication

        _ = builder
            .Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = DiscordAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "auth_cookie";
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.MaxAge = TimeSpan.FromDays(30);
                options.LoginPath = "/api/auth/login";
                options.LogoutPath = "/api/auth/logout";
                options.AccessDeniedPath = "/api/auth/access-denied";
            })
            .AddDiscord(options =>
            {
                options.ClientId =
                    builder.Configuration["Authentication:Discord:ClientId"]
                    ?? throw new ArgumentNullException("Discord ClientId is missing");
                options.ClientSecret =
                    builder.Configuration["Authentication:Discord:ClientSecret"]
                    ?? throw new ArgumentNullException("Discord ClientSecret is missing");

                options.Scope.Add("identify");

                options.CallbackPath = "/api/auth/signin-discord";

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var discordUser = context.User;
                        string discordId =
                            discordUser.GetProperty("id").GetString()
                            ?? throw new ArgumentNullException("Discord ID is missing");
                        string username =
                            discordUser.GetProperty("username").GetString()
                            ?? throw new ArgumentNullException("Discord Username is missing");

                        var userService =
                            context.HttpContext.RequestServices.GetRequiredService<IUserService>();

                        var user = await userService.GetUserByDiscordIdAsync(
                            ulong.Parse(discordId)
                        );
                        if (user == null)
                        {
                            user = await userService.CreateUserAsync(
                                ulong.Parse(discordId),
                                username
                            );
                        }
                        else
                        {
                            user.Name = username;
                            user = await userService.UpdateUserAsync(user);
                        }
                    }
                };
            });

        _ = builder.Services.AddAuthorization();

        #endregion Authentication

        return builder;
    }
}
