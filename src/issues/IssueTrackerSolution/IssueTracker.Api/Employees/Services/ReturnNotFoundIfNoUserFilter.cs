namespace IssueTracker.Api.Employees.Services;

/// <summary>
///  For a demo.
/// It might be a good idea in some cases, security-wise, to not return a 401, but a 404.
/// You don't even want the bad guy to know that they "found a thing".
/// </summary>
public class ReturnNotFoundIfNoUserFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.HttpContext.User.Identity is { IsAuthenticated: false })
        {
            return TypedResults.NotFound();
        }
        return await next(context);
    }
}
