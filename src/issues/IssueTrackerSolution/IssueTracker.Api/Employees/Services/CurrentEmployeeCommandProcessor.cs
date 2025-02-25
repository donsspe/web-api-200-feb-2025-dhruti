using IssueTracker.Api.Employees.Api;
using IssueTracker.Api.Employees.Domain;

namespace IssueTracker.Api.Employees.Services;

public class CurrentEmployeeCommandProcessor(IHttpContextAccessor context) : IProcessCommandsForTheCurrentEmployee
{
    public Task<ProblemSubmitted> ProcessProblemAsync(SubmitProblem problem)
    {
        var sub = context?.HttpContext?.User.Identity?.Name ?? throw new Exception("This should never happen");

        // but we need the employee to do this,
        // we need the sub claim
        // look it up in the database, if isn't there, create it
        // if it is there, load, tell it process this command, 
        // save it. 
        // return the ProblemSubmitted.
        throw new Exception();
    }
}
