
using Dapper;
using Npgsql;

namespace IssueTracker.Api.Employees.Services;

public class SoftwareMustExistInCatalogEndpointFilter(NpgsqlConnection connection, ILogger<SoftwareMustExistInCatalogEndpointFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // take a look at the parameter in the url called softwareId
        // if it isn't there, this thing is being used incorrectly. A developer messed up.
        // if it is there, check to see if we have that software, and if it's there, do nothing (next(context))
        // if it isn't, return a 404.
        var softwareId = context.HttpContext.GetRouteValue("softwareId");
        if (softwareId is null)
        {
            logger.LogError("The filter was used on a path without a softwareId");

            return TypedResults.NotFound();
        }

        var sql = "SELECT EXISTS(SELECT 1 from catalog Where Id=uuid(:id))";
        var paramMap = new { id = softwareId };
        var softwareExists = await connection.ExecuteScalarAsync<bool>(sql, paramMap);
        if (softwareExists == false)
        {
            return TypedResults.NotFound("Software not in the catalog, can't create a problem for it");
        }

        return await next(context); // I'm cool, carry on, call the method everything good here.
    }
}
