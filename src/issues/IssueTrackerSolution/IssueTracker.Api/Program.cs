using IssueTracker.Api;
using IssueTracker.Api.Catalog.Api;
using IssueTracker.Api.Utils;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.AddIssueTrackerServices();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("docs", c =>
    {
        c.Title = "Issue Tracker API";
      
    });
}

app.MapIssueTracker();


app.Run();

public partial class Program;
