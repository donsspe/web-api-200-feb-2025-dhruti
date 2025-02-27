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
        // etc.
        // var location = postResponse.Context.Response.Headers.Location.First()!;
        //
        // var getResponse = await fixture.Host.Scenario(api =>
        // { 
        //     api.WithClaim(new Claim("sub", "byron"));
        //     api.Get.Url(location);
        //     api.StatusCodeShouldBe(200);
        // });
        //
        // var getBody = getResponse.ReadAsJson<EmployeeProblemReadModel>();
        //
        // getBody.ShouldBe(postBody);
    }

    [Fact]
    public async Task OnlyAuthenticatedEmployeesCanSubmit()
    {
        
        // Todo: All requests using this fixture are authenticated - consider a different fixture for this test which returns a 401 for all endpoints.
        var postModel = new ProblemSubmitModel("Has issues");
        var postResponse = await fixture.Host.Scenario(api =>
        {
            api.Post.Json(postModel).ToUrl($"/employee/software/{SeededSoftware.Rider}/problems");
            api.StatusCodeShouldBe(403);

        });
    }
    
}

public class ProblemSubmissionFixture : HostedUnitIntegrationTestFixture
{
    
}

[CollectionDefinition("SubmittingProblems")]
public class ProblemSubmissionCollection : ICollectionFixture<ProblemSubmissionFixture>
{
    
}