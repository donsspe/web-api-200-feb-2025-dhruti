
using IssueTracker.Api.Employees.Services;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IssueTracker.Api.Employees.Api;

public static class SubmittingAProblem
{
    public static async Task<Created<EmployeeProblemReadModel>> SubmitAsync(
        ProblemSubmitModel request,
        Guid softwareId,
        IProvideTheEmployeeId employeeIdProvider,
        IDocumentSession session,
    CancellationToken token,
    HttpContext context
    )
    {

        var employeeId = await employeeIdProvider.GetEmployeeIdAsync(token);
        var problemId = Guid.NewGuid();
        var employeeProblem = new EmployeeSubmittedAProblem(problemId, employeeId, softwareId, request.Description);

        session.Events.StartStream(problemId, employeeProblem);
        await session.SaveChangesAsync(token);

        var response = await session.LoadAsync<EmployeeProblemReadModel>(problemId, token);
        return TypedResults.Created($"/employee/software/{response!.SoftwareId}/problems/{response.Id}", response);
    }
}


public record ProblemSubmitModel(string Description);

public record EmployeeSubmittedAProblem(Guid ProblemId, Guid EmployeeId, Guid SoftwareId, string Description);