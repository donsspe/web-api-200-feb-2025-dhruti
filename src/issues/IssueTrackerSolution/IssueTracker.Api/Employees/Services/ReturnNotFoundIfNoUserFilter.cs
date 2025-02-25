using Microsoft.AspNetCore.Mvc.Filters;

namespace IssueTracker.Api.Employees.Services;

/// <summary>
///  For a demo - maybe later. Use at your own discretion;
/// </summary>
public class ReturnNotFoundIfNoUserFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if(context.HttpContext.User is null)
        {
            return TypedResults.NotFound();
        }
        return await next(context);
    }
}
