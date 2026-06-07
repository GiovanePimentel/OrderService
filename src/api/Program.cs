using System.Text.Json.Nodes;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<OrderService>();
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.Migrate();
}

/*
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

App.UseHttpsRedirection();
*/
app.MapGet("/", () => "API Funcionando");
app.MapGet("/weatherforecast", () =>
{
    return new
    {
        Status = "OK",
        Data = DateTime.Now
    };
});
app.MapGet("/db-test", (AppDbContext db) =>
{
    return Results.Ok("contexto em funcionamento");
});
app.Run();

