

using IssueTracker.Api.Employees.Domain;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IssueTracker.Api.Employees.Api;

public static class SubmittingAProblem
{
    public static async  Task<Ok> SubmitAsync(
        ProblemSubmitModel request,
        Guid softwareId,
        IProcessCommandsForTheCurrentEmployee employeeCommandProcessor,
        CancellationToken token
        )
    {
        
        
        var problem = new SubmitProblem(softwareId, request.Description);
   
        ProblemSubmitted response = await employeeCommandProcessor.ProcessProblemAsync(problem);



        return TypedResults.Ok();
    }
}


public record ProblemSubmitModel(string Description);

public interface IProcessCommandsForTheCurrentEmployee
{
 
    Task<ProblemSubmitted> ProcessProblemAsync(SubmitProblem problem);
}