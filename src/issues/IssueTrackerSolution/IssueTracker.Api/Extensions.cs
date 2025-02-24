using IssueTracker.Api.Catalog.Api;
using Marten;
using Npgsql;


namespace IssueTracker.Api;

public static class Extensions
{
    public static IHostApplicationBuilder AddIssueTrackerServices(this IHostApplicationBuilder host)
    {
        var services = host.Services;
        
        services.AddSingleton<TimeProvider>(_ => TimeProvider.System);
        
        services.AddAuthorization();
        services.AddAuthentication().AddJwtBearer();
        
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
  
        return endpoints;
    }
    
   
}