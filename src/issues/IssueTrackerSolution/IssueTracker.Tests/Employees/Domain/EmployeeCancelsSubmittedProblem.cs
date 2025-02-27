using IssueTracker.Api.Employees.Api;
using IssueTracker.Api.Middleware;
using IssueTracker.Tests.Fixtures;
using Shouldly;

namespace IssueTracker.Tests.Employees.Domain;

[Trait("Category", "UnitIntegration")]
[Collection("UnitIntegration")]
public class EmployeeCancelsSubmittedProblem(UnitIntegrationTestFixture fixture)
{
    [Fact(Skip = "Fix this later")]
    public async Task Sketch()
    {
        // if an employee
        // let's create a new employee
        await using var session = fixture.Store.LightweightSession(); // get a connection to your local or dev database

        var createEvent = new EmployeeCreated(Guid.NewGuid(), "carlos");

        session.Events.StartStream(createEvent.Id, createEvent);


        //
        // has submitted a problem, 
        var employeeSubmittedProblemEvent = new EmployeeSubmittedAProblem(Guid.NewGuid(), createEvent.Id, Guid.Parse(SeededSoftware.Rider), "Broken");

        session.Events.StartStream(employeeSubmittedProblemEvent.ProblemId, employeeSubmittedProblemEvent);

        await session.SaveChangesAsync();
        // and the problem is still in the status of submitted,

        var readModelBeforeCancel = await session.Events.AggregateStreamAsync<EmployeeProblemReadModel>(employeeSubmittedProblemEvent.ProblemId);
        Assert.NotNull(readModelBeforeCancel);
        readModelBeforeCancel.Status.ShouldBe("Submitted");

        // Magic!
        session.Events.Append(employeeSubmittedProblemEvent.ProblemId, new ProblemCancelledByUser());
        await session.SaveChangesAsync();
        // After
        var readModelAfterCancel = await session.Events.AggregateStreamAsync<EmployeeProblemReadModel>(employeeSubmittedProblemEvent.ProblemId);
        Assert.Null(readModelAfterCancel);





        // have the employee submit a problem
        // then it should be cancelled (removed, gone, whatever)
        // check the read model?

        // If it isn't in that state, it should stay.
    }
}
