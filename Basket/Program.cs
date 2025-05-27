var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.AddRedisDistributedCache(connectionName: "cache");// ConnectionStrings__cache

builder.Services.AddScoped<BasketCommands>();
builder.Services.AddScoped<BasketQueries>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.Run();