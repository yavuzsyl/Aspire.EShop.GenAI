using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

builder.Services.AddRateLimiter(options =>
    {
        options.AddFixedWindowLimiter("fixed", options =>
        {
            options.Window = TimeSpan.FromSeconds(10);
            options.PermitLimit = 2;
        });
    });

var app = builder.Build();

app.UseRateLimiter();

app.MapReverseProxy();

app.Run();