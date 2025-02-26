using System.Security.Claims;
using IssueTracker.Api.Employees.Api;
using IssueTracker.Api.Employees.Domain;
using IssueTracker.Api.Middleware;
using JasperFx.Core;

namespace IssueTracker.Api.Employees.Services;

public class CurrentEmployeeCommandProcessor(IHttpContextAccessor context,
    EmployeeRepository repository) : IProcessCommandsForTheCurrentEmployee
{
    public async Task<ProblemSubmitted> ProcessProblemAsync(SubmitProblem problem)
    {
        // we need the sub claim
        var sub = context?.HttpContext?.User.FindFirstValue("sub") ?? throw new ChaosException("This should never happen");
  
        var employee = await repository.GetBySubAsync(sub);
        if (employee is null)
        {
            employee = repository.Create(sub);
            await employee.SaveAsync();
            
        }
       var result = employee.Process(problem);
       await employee.SaveAsync();
        return result;
    }
}
