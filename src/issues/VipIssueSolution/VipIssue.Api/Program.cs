using Marten;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Start - Add connection string for Database setup
var connectionString = builder.Configuration.GetConnectionString("postgres") ?? throw new Exception("Need a connection string");
builder.Services.AddMarten(config =>
{
    config.Connection(connectionString);
}).UseLightweightSessions();
//End

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.MapPost("/vips/notifications", async (VipIssueCreateModel request, IDocumentSession session) =>
{
    //return TypedResults.Ok();
    //return TypedResults.Created("/tacos", new { name = "dhruti" });
    //return TypedResults.Created("/tacos", request);
    var response = new VipIssueResponseModel(
            Guid.NewGuid(),
            request.Problem,
            request.Description,
            "Pending"
        );
    //return TypedResults.Created("/tacos", response);
    
    session.Insert(response); //Store in database
    await session.SaveChangesAsync();
    return TypedResults.Created($"/vip/notifications/{response.Id}", response);
});

//app.MapGet("/vip/notifications/{id:guid}", () => TypedResults.Ok());
app.MapGet("/vip/notifications/{id:guid}", async (Guid id, IDocumentSession session) =>
{
    //var response = new VipIssueResponseModel(
    //    id, "", "", "Pending");
    var response = await session.LoadAsync<VipIssueResponseModel>(id);
    return TypedResults.Ok(response);
});


app.Run();

public partial class Program { }

public record VipIssueCreateModel(string Problem, string Description);

public record VipIssueResponseModel(Guid Id, string Problem, string Description, string Status);