
using System.ComponentModel;
using IssueTracker.Api.Employees.Domain;
using IssueTracker.Tests.Fixtures;

namespace IssueTracker.Tests.Employees.Domain;
[Trait("Category", "UnitIntegration")]
[Collection("UnitIntegration")]
public class CurrentEmployeeCommandProcessorTests(UnitIntegrationTestFixture fixture)
{
    [Fact(Skip = "Delete this... for demonstration purposes...")]
    public async Task NoEmployeeExistsButIsCreated()
    {
        var session = fixture.Store.LightweightSession();
        var repository = new EmployeeRepository(session);
        
        
    }
}
