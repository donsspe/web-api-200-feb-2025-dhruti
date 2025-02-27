using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IssueTracker.Api.Employees.Api;

public static class CancellingSubmittedProblems
{

    public static async Task<Results<NoContent, Conflict<string>>> CancelAProblem(

        Guid problemId,
        IDocumentSession session,
        CancellationToken token
        )
    {
        // Note - I'm going to do this fast, and not doing all the checkes and stuff

        var problem = await session.LoadAsync<EmployeeProblemReadModel>(problemId);

        if (problem is null)
        {
            return TypedResults.NoContent();
        }

        if (problem.Status != "Submitted")
        {
            return TypedResults.Conflict("Cannot be cancelled in this state");
        }
        session.Events.Append(problemId, new ProblemCancelledByUser());

        await session.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}
