using System.Security.Claims;
using Infrastructure;

namespace Presentation;

public class SetCurrentUserMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
    {
        // Check if the user is authenticated
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            // Attempt to get the user's ID from a claim.
            // Here we assume the user's ID is stored in the NameIdentifier claim.
            var userIdClaim = context.User.FindFirst("ApplicationUserId");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                // Set the current user ID on your DbContext so that global query filters can use it.
                dbContext.CurrentUserId = userId;
            }
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }
}
