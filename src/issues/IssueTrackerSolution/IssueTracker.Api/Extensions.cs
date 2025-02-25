using IssueTracker.Api.Catalog.Api;
using IssueTracker.Api.Employees.Api;
using IssueTracker.Api.Employees.Services;
using Marten;
using Npgsql;


namespace IssueTracker.Api;

public static class Extensions
{
    public static IHostApplicationBuilder AddIssueTrackerServices(this IHostApplicationBuilder host)
    {
        var services = host.Services;
        
        // .net 8 and forward - good idea.
        services.AddSingleton<TimeProvider>(_ => TimeProvider.System);

        services.AddScoped<IProcessCommandsForTheCurrentEmployee, CurrentEmployeeCommandProcessor>();
        services.AddAuthorization();
        services.AddAuthentication().AddJwtBearer();

        // We'll use this later, for when our aggregates need to the context.
        services.AddHttpContextAccessor();
        
        var connectionString = host.Configuration.GetConnectionString("postgres") ?? throw new InvalidOperationException("No connection string found");

        var npgDataSource = NpgsqlDataSource.Create(connectionString);
        
        services.AddNpgsqlDataSource(connectionString);

        services.AddMarten(config =>
        {
            config.Connection(connectionString);

        }).UseNpgsqlDataSource().UseLightweightSessions();

   
        return host;
    }
    
    public static IEndpointRouteBuilder MapIssueTracker(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCatalog();
        endpoints.MapEmployees();
  
        return endpoints;
    }
    
   
}