using IssueTracker.Api.Employees.Api;
using IssueTracker.Api.Middleware;
using IssueTracker.Tests.Fixtures;
using Shouldly;
namespace IssueTracker.Tests.Employees.Domain;

[Trait("Category", "UnitIntegration")] // this is metadata so we can run just some of these at a time.
[Collection("UnitIntegration")] // everything here should use the same database as everything else in this "collection"
public class EmployeeSubmitsAProblem(UnitIntegrationTestFixture fixture) : IAsyncLifetime
{
    private readonly string _newSub = "carl";
    private readonly Guid _employeeId = Guid.NewGuid();
    private readonly Guid _problemId = Guid.NewGuid();
    private readonly Guid _softwareId = Guid.Parse(SeededSoftware.DockerDesktop);
    private readonly string _description = "It doesn't work!";
    private EmployeeProblemReadModel? _problemReadModelReadModel = null!;

    public async Task InitializeAsync()
    {
        await using var session = fixture.Store.LightweightSession();

        var user = new EmployeeCreated(_employeeId, _newSub);
        session.Events.StartStream(user.Id, user);

        var problem = new EmployeeSubmittedAProblem(_problemId, _employeeId, _softwareId, _description);
        session.Events.StartStream(_problemId, problem);
        await session.SaveChangesAsync();

        _problemReadModelReadModel = (await session.Events.AggregateStreamAsync<EmployeeProblemReadModel>(_problemId))!;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void HasCopiedEventData()
    {
        _problemReadModelReadModel.Description.ShouldBe(_description);
        _problemReadModelReadModel.EmployeeId.ShouldBe(_employeeId);
        _problemReadModelReadModel.SoftwareId.ShouldBe(_softwareId);
        _problemReadModelReadModel.Id.ShouldBe(_problemId);
    }

}
