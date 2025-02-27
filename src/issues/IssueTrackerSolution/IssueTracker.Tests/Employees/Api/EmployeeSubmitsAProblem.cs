using System.Security.Claims;
using IssueTracker.Api.Employees.Api;
using IssueTracker.Tests.Fixtures;
using Shouldly;

namespace IssueTracker.Tests.Employees.Api;
[Collection("SubmittingProblems")]
[Trait("Category", "System")]
public class EmployeeSubmitsAProblem(ProblemSubmissionFixture fixture)
{

    [Fact]
    public async Task SubmittedIssuesMustBeForSoftwareInTheCatalog()
    {
        var postModel = new ProblemSubmitModel("Has issues");
        var postResponse = await fixture.Host.Scenario(api =>
        {
            api.WithClaim(new Claim("sub", "byron"));
            api.Post.Json(postModel).ToUrl($"/employee/software/{SeededSoftware.NotPresentInCatalog}/problems");
            api.StatusCodeShouldBe(404);

        });

    }
    [Fact]
    public async Task SubmittingAProblem()
    {
        var postModel = new ProblemSubmitModel("Has issues");
        var postResponse = await fixture.Host.Scenario(api =>
       {
           api.WithClaim(new Claim("sub", "byron"));
           api.Post.Json(postModel).ToUrl($"/employee/software/{SeededSoftware.Rider}/problems");
           api.StatusCodeShouldBe(201);

       });

        var postBody = postResponse.ReadAsJson<EmployeeProblemReadModel>();
        postBody.Description.ShouldBe("Has issues");
        postBody.Status.ShouldBe("Submitted");
        postBody.Opened.ShouldBe(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        //  etc.
        var location = postResponse.Context.Response.Headers.Location.First()!;

        var getResponse = await fixture.Host.Scenario(api =>
        {
            api.WithClaim(new Claim("sub", "byron"));
            api.Get.Url(location);
            api.StatusCodeShouldBe(200);
        });

        var getBody = getResponse.ReadAsJson<EmployeeProblemReadModel>();

        getBody.ShouldBe(postBody);
    }



}

public class ProblemSubmissionFixture : HostedUnitIntegrationTestFixture
{

}

[CollectionDefinition("SubmittingProblems")]
public class ProblemSubmissionCollection : ICollectionFixture<ProblemSubmissionFixture>
{

}