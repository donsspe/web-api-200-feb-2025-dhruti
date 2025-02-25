var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build(); // Life before this line (configuring services) and life after this.
// from here on, the services collection can not be modified.
// builder.Services.AddSingleton(_ => TimeProvider.System); // We can't placed this line here after builder.build(), we can place before that line
// all about what to expose and how to handle incoming requests
// "Middleware, Controllers, Minimal APIs, etc."

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// HTTP default port is 80
// HTTPS default port is 443
app.UseHttpsRedirection(); // if they get here using HTTP, redirect them to the HTTPs version

app.UseAuthorization();

app.MapControllers(); // we need this line, if you choose controller by adding this line above "builder.Services.AddControllers();"

app.MapGet("/cinnamon-roll", () => "Also Delicious"); //--> This is another way to set up controller end point with out creating controller exclusivly.

app.Run(); // this is a blocking call that will "spin" forever....
