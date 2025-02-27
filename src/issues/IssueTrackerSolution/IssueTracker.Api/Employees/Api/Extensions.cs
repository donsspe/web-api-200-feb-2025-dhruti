using IssueTracker.Api.Employees.Services;
using IssueTracker.Api.Middleware;

namespace IssueTracker.Api.Employees.Api;

public static class Extensions
{
    public static IEndpointRouteBuilder MapEmployees(this IEndpointRouteBuilder routes)
    {
        var employeeGroup = routes.MapGroup("employee")
            .WithTags("Employees") // just add this to the documentation (openapi)
            .WithDescription("Employee Related Stuff")
            .RequireAuthorization() // simply make sure they have a valid token

            .AddEndpointFilter<AuthenticatedUserToEmployeeMiddleware>();

        employeeGroup.MapPost("/software/{softwareId:guid}/problems", SubmittingAProblem.SubmitAsync)
            .AddEndpointFilter<SoftwareMustExistInCatalogEndpointFilter>();

        employeeGroup.MapGet("/software/{softwareId:guid}/problems/{problemId:guid}", GettingEmployeeProblems.GetProblem);
        // GET /employees/software [all their entitled software]
        employeeGroup.MapGet("/software/{softwareId:guid}/problems", GettingEmployeeProblems.GetAllProblems);

        employeeGroup.MapDelete("/software/{softwareId:guid}/problems/{problemId:guid}", CancellingSubmittedProblems.CancelAProblem);



        return routes;
    }
}
