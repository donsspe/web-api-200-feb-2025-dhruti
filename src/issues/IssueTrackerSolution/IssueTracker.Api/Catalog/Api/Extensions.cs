using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;

namespace IssueTracker.Api.Catalog.Api;

public static class Extensions
{
    public static IEndpointRouteBuilder MapCatalog(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/catalog", GetCatalogAsync).WithTags("Catalog").WithDescription("Get all catalog items")
            .WithDisplayName("Catalog Items");

        return endpoints;
    }

    private static async Task<Ok<IEnumerable<CatalogItem>>> GetCatalogAsync(NpgsqlConnection connection,
        CancellationToken token)
    {
        var sql = "SELECT id, title, description, vendor FROM catalog";
        var catalogItems = await connection.QueryAsync<CatalogItem>(sql, token);
        return TypedResults.Ok(catalogItems);
    }
}

public record CatalogItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Vendor { get; set; } = string.Empty;
}