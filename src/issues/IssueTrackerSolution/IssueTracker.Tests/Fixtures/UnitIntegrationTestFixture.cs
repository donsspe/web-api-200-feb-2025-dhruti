using JasperFx.CodeGeneration;
using Marten;
using Testcontainers.PostgreSql;
using Weasel.Core;

namespace IssueTracker.Tests.Fixtures;


// You would create one of these to use for your UnitIntegration Tests
// You don't use these on "unit" tests.
// 

public class UnitIntegrationTestFixture : IAsyncLifetime
{
    public DocumentStore Store { get; private set; } = null!;

    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .Build();
  
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        
        Store = DocumentStore.For(opts =>
        {
            opts.Connection(_container.GetConnectionString());
            opts.AutoCreateSchemaObjects = AutoCreate.All;
            opts.GeneratedCodeMode = TypeLoadMode.Auto;
            opts.ApplicationAssembly = GetType().Assembly;
        });
        
    }

    public  Task DisposeAsync()
    {
        Store.Dispose();
        return _container.DisposeAsync().AsTask();
    }
}

[CollectionDefinition("UnitIntegration")]
public class UnitIntegrationCollection : ICollectionFixture<UnitIntegrationTestFixture>
{
}