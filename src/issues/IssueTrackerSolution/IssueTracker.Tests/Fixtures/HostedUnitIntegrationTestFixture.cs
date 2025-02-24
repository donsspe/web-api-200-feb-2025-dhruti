using Alba;
using Alba.Security;
using JasperFx.CodeGeneration;
using Marten;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Weasel.Core;

namespace IssueTracker.Tests.Fixtures;

public abstract class HostedUnitIntegrationTestFixture : IAsyncLifetime
{
    public DocumentStore? Store { get; private set; }
    public IAlbaHost Host { get; private set; } = null!;
    private PostgreSqlContainer _container = null!;

    public async Task InitializeAsync()
    {
        if (UseTestContainer)
        {
            _container = new PostgreSqlBuilder()

                .Build();
            await _container.StartAsync();
            await _container.ExecScriptAsync(Scripts.SeedCatalog);

            Store = DocumentStore.For(opts =>
            {
                opts.Connection(_container.GetConnectionString());
                opts.AutoCreateSchemaObjects = AutoCreate.All;
                opts.GeneratedCodeMode = TypeLoadMode.Auto;
                opts.ApplicationAssembly = GetType().Assembly;
            });
        }

        Host = await AlbaHost.For<Program>(config =>
        {
            if (UseTestContainer)
            {
                config.UseSetting("ConnectionStrings:postgres", _container.GetConnectionString());
            }

            config.ConfigureServices(ConfigureServices);
            config.ConfigureTestServices(ConfigureTestServices);
        }, GetAuthenticationStub());

        await BeforeAsync();
    }

    /// <summary>
    /// Use this to seed data, or perform other setup actions.
    /// </summary>
    /// <returns></returns>
    protected virtual Task BeforeAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Set this to true to skip the test container setup. It will use the running database.
    /// </summary>
    protected virtual bool UseTestContainer => true;

    /// <summary>
    /// Add new services not yet present in the services collection
    /// </summary>
    /// <param name="services"></param>
    protected virtual void ConfigureServices(IServiceCollection services)
    {
    }

    /// <summary>
    /// Override the services in the services collection. Called after ConfigureServices
    /// </summary>
    /// <param name="services"></param>
    protected virtual void ConfigureTestServices(IServiceCollection services) {}

protected virtual AuthenticationStub GetAuthenticationStub()
    {
        return new();
    }

    public async Task DisposeAsync()
    {
        if (UseTestContainer)
        {
            await Store!.DisposeAsync();
            await _container.DisposeAsync();
        }

        await Host.DisposeAsync();
    }
}