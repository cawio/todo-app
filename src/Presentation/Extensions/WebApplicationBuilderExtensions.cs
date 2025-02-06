using System.Text.Json;
using System.Text.Json.Serialization;
using Application;
using Infrastructure;
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
            options.AddDefaultPolicy(builder =>
            {
                _ = builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
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

        return builder;
    }
}
