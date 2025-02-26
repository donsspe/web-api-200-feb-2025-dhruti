using IssueTracker.Api.Employees.Domain;
using IssueTracker.Tests.Fixtures;
using Marten;

namespace IssueTracker.Tests.Employees.Domain;

[Trait("Category", "UnitIntegration")] // this is metadata so we can run just some of these at a time.
[Collection("UnitIntegration")] // everything here should use the same database as everything else in this "collection"
public class EmployeeTests(UnitIntegrationTestFixture fixture)
{
    [Fact]

    public async Task CanCreateAnEmployee()
    {
        IDocumentSession session = fixture.Store.LightweightSession(); 


        var repository = new EmployeeRepository(session); // a thing that handles persistence.
        var sub = "bob@company"; // this is in the JWT
        var emp = repository.Create(sub);

        // I want to save it to the database (we do this through the aggregate)
        await emp.SaveAsync();
        // and make sure it got saved.

        var emp2 = await repository.GetByIdAsync(emp.Id);

        Assert.NotNull(emp2);
        Assert.Equal(emp.Id, emp2.Id);

        var emp3 = await repository.GetBySubAsync(sub);
       
        Assert.NotNull(emp3);
        // Assert.Equal(/// No sub ?? We should probably check that)

        // back door where I can check to make sure the "technical" requirements are met.

        var loadedEntity = await session.LoadAsync<EmployeeEntity>(emp.Id);

        Assert.NotNull(loadedEntity);
        Assert.Equal(loadedEntity.Sub, sub);

    }

    [Fact]
    public async Task EmployeeCanSubmitProblems()
    {

        // Step 1 - "Given"
        IDocumentSession session = fixture.Store.LightweightSession();


        var repository = new EmployeeRepository(session); // a thing that handles persistence.
        var sub = "sue@company";
        var emp = repository.Create(sub);

        var problemToSubmit = new SubmitProblem(Guid.NewGuid(), "Blammo");

        // Step 2 - "When"
        var theEvent = emp.Process(problemToSubmit);
        // we should probably save the thing??

        await emp.SaveAsync();

        // Step 3 - "Then"


        var loadedEmp = await session.LoadAsync<EmployeeEntity>(emp.Id);

        Assert.NotNull(loadedEmp);

        Assert.Single(loadedEmp.Problems);

        var savedProblem = loadedEmp.Problems.First();

        Assert.Equal(theEvent, savedProblem);

    }
}
