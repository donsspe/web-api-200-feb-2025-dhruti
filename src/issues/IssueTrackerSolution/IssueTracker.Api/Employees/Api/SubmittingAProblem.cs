

using IssueTracker.Api.Employees.Domain;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IssueTracker.Api.Employees.Api;

public static class SubmittingAProblem
{
    public static async  Task<Ok> SubmitAsync(
        ProblemSubmitModel request,
        Guid softwareId,
        IProvideEmployeesForTheApi employeeProvider,
        CancellationToken token
        )
    {
        // look up in the database to make sure we have the software with that id.
        // if not, return an error (404)
        
        var problem = new SubmitProblem(softwareId, request.Description);
        //var emp = new Employee.Domain.Employee(null, null);
        Employee emp = await employeeProvider.GetCurrentEmployeeAsync();
        var problemSubmitted = emp.Process(problem);
        await emp.SaveAsync();
      
        //var problemSubmitted = emp.Process(problem);
     
        return TypedResults.Ok();
    }
}


public record ProblemSubmitModel(string Description);

public interface IProvideEmployeesForTheApi
{
    Task<Employee> GetCurrentEmployeeAsync();
}