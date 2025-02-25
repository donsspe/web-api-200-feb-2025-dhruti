

using Alba.Security;
using IssueTracker.Api.Employees.Api;
using IssueTracker.Api.Employees.Domain;
using IssueTracker.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;


namespace IssueTracker.Tests.Employees.Api;
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

    [Fact]
    public async Task SoftwareNotIntheCatalogReturnsFourOhFour()
    {
        var problem = new ProblemSubmitModel("Thing is broke real bad");
        
        var response = await fixture.Host.Scenario(api =>
        {
            api.Post
            .Json(problem)
            .ToUrl($"/employee/software/{SeededSoftware.NotPresentInCatalog}/problems");
            api.StatusCodeShouldBe(404);
            
        });
    }

}



public class EmployeeHostedIntegrationTest : HostedUnitIntegrationTestFixture
{
    protected override AuthenticationStub GetAuthenticationStub()
    {
        return new AuthenticationStub().WithName("bob@company.com");
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        var fakeEmloyeeProvider = Substitute.For<IProcessCommandsForTheCurrentEmployee>();
        var fakeEmployeeEntity = new EmployeeEntity
        {
            Id = Guid.NewGuid()
        };
        fakeEmloyeeProvider.ProcessProblemAsync(Arg.Any<SubmitProblem>()).Returns(Task.FromResult(new ProblemSubmitted(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "tacos", DateTimeOffset.UtcNow)));

        services.AddScoped<IProcessCommandsForTheCurrentEmployee>(_ => fakeEmloyeeProvider);
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);
    }
}

[CollectionDefinition("EmployeeApiCollection")]
public class EmployeeApiFixture: ICollectionFixture<EmployeeHostedIntegrationTest> { }