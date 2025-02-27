using IssueTracker.Api.Employees.Api;
using Marten;

namespace IssueTracker.Api.VipApiEndpoints;

public static class Extensions
{

    public static IEndpointRouteBuilder MapVipApiEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("vipapi").WithTags("VipEndpoints");

        group.MapGet("/problems/{problemId:guid}", async (IDocumentSession session, Guid problemId) =>
        {
            var response = await session.Events.AggregateStreamAsync<VipIssueReadModel>(problemId);
            return TypedResults.Ok(response);
        });

        return group;
    }
}

/* {
  "problem": "http://us/vipapi/problems/6bbe43c5-337a-4695-9875-a70985c8778a",
  "description": "My stuff is broken again! Help!"
} */

public record VipIssueReadModel
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public static VipIssueReadModel Create(EmployeeSubmittedAProblem @event)
    {
        return new VipIssueReadModel
        {
            Id = @event.ProblemId,
            Description = @event.Description,
            EmployeeId = @event.EmployeeId,
            Status = "Submitted"
        };
    }

    public static VipIssueReadModel Apply(ProblemCancelledByUser @event, VipIssueReadModel model) => model with { Status = "Employee Cancelled" };

}

