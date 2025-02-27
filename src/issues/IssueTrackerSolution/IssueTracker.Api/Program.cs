using IssueTracker.Api;
using IssueTracker.Api.Middleware;
using IssueTracker.Api.Utils;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.AddIssueTrackerServices();

var app = builder.Build();



app.UseMiddleware<GlobalChaosExceptionHandler>();

app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // app.UseSwagger(); &&
                      // app.UseSwaggerUi();
    app.MapScalarApiReference("docs", c =>
    {
        c.Title = "Issue Tracker API";

    });
}

var routes = app.MapGroup("");

routes.MapIssueTracker();


app.Run();

public partial class Program;
