using IssueTracker.Api.Employees.Services;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IssueTracker.Api.Employees.Api;

public static class GettingEmployeeProblems
{

    public static async Task<Results<Ok<EmployeeProblemReadModel>, NotFound>> GetProblem(
        IProvideTheEmployeeId employeeProvider,
        IDocumentSession session,
        Guid softwareId,
        Guid problemId,
        CancellationToken token
        )
    {

        var employeeId = await employeeProvider.GetEmployeeIdAsync();
        var problem = await session.Query<EmployeeProblemReadModel>().Where(p => p.Id == problemId && p.SoftwareId == softwareId && p.EmployeeId == employeeId).SingleOrDefaultAsync();
        if (problem is null)
        {
            return TypedResults.NotFound();
        }
        else
        {
            return TypedResults.Ok(problem);
        }
    }
    public static async Task<Ok<IReadOnlyList<EmployeeProblemReadModel>>> GetAllProblems(
        IProvideTheEmployeeId employeeProvider,
        IDocumentSession session,
        Guid softwareId,
        CancellationToken token)
    {

        var employeeId = await employeeProvider.GetEmployeeIdAsync();

        var problems = await session.Query<EmployeeProblemReadModel>().Where(p => p.EmployeeId == employeeId && p.SoftwareId == softwareId).ToListAsync();

        return TypedResults.Ok(problems);
    }
}
