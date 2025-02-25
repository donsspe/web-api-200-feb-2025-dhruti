

using Alba.Security;
using IssueTracker.Api.Employee.Api;
using IssueTracker.Tests.Fixtures;
using JasperFx.CodeGeneration.Frames;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IssueTracker.Tests.Employee.Api;
[Trait("Category", "UnitIntegration")]
[Collection("EmployeeApiCollection")]
public class SubmittingAProblem(EmployeeHostedIntegrationTest fixture)
{
    [Fact]
    public async Task Trial()
    {

        // the employee identity (that has to be in the authorization header as a JWT)
        // the software id (that is in the URL - route parameter, softwareId
        // the description of the problem.

        var problem = new ProblemSubmitModel("Thing is broke real bad");
        var response = await fixture.Host.Scenario(api =>
        {
            api.Post
            .Json(problem)
            .ToUrl($"/employee/software/{SeededSoftware.DockerDesktop}/problems");
        });

        //var responseBody = response.ReadAsJson<SubmittingAProblem>();
        //Assert.NotNull(responseBody);

        // get the employee id 
        // verify the software exists
        // figure out what the API should return for this.


    }
}



public class EmployeeHostedIntegrationTest : HostedUnitIntegrationTestFixture
{
    protected override AuthenticationStub GetAuthenticationStub()
    {
        return new AuthenticationStub().WithName("bob@company.com");
    }
}

[CollectionDefinition("EmployeeApiCollection")]
public class EmployeeApiFixture: ICollectionFixture<EmployeeHostedIntegrationTest> { }