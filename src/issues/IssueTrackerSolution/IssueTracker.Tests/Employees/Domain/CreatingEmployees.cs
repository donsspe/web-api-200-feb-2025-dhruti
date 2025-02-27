using IssueTracker.Api.Middleware;
using IssueTracker.Tests.Fixtures;
using Shouldly;

namespace IssueTracker.Tests.Employees.Domain;

[Trait("Category", "UnitIntegration")] // this is metadata so we can run just some of these at a time.
[Collection("UnitIntegration")] // everything here should use the same database as everything else in this "collection"
public class CreatingEmployees(UnitIntegrationTestFixture fixture) : IAsyncLifetime
{
    private readonly string _newSub = "joey";
    private readonly Guid _newId = Guid.NewGuid();
    private Employee _userReadModel = null!;
    public async Task InitializeAsync()
    {
        await using var session = fixture.Store.LightweightSession();

        var user = new EmployeeCreated(_newId, _newSub);
        
        session.Events.StartStream(user.Id, user);
        
        await session.SaveChangesAsync();

        _userReadModel = (await session.Events.AggregateStreamAsync<Employee>(user.Id))!;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    
    [Fact]
    public void HasCorrectIdAndSub()
    {
        _userReadModel.Sub.ShouldBe(_newSub);
        _userReadModel.Id.ShouldBe(_newId);
       
    }

    [Fact]
    public void HasCorrectCreationDate()
    {
        _userReadModel.EmployeeCreated.ShouldBe(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }
    [Fact]
    public void HasCorrectLastApiUsage()
    {
        _userReadModel.LastApiUsage.ShouldBe(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }
    

}