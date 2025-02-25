

using IssueTracker.Api.Employee.Domain;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IssueTracker.Api.Employee.Api;

public static class SubmittingAProblem
{
    public static async  Task<Ok> SubmitAsync(
        ProblemSubmitModel request,
        HttpContext context,
        Guid softwareId,
        CancellationToken token
        )
    {
       
        var problem = new SubmitProblem(softwareId, request.Description);

     
        return TypedResults.Ok();
    }
}


public record ProblemSubmitModel(string Description);