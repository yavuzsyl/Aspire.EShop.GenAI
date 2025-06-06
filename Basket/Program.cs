var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.AddRedisDistributedCache(connectionName: "cache");// ConnectionStrings__cache

builder.Services.AddScoped<BasketCommands>();
builder.Services.AddScoped<BasketQueries>();
builder.Services.AddHttpClient<CatalogApiClient>(client =>
{
    client.BaseAddress = new("http+https://catalog");
});

builder.Services.AddMassTransitWithAssemblies(Assembly.GetExecutingAssembly());

builder.Services.AddAuthentication()
                .AddKeycloakJwtBearer(
                    serviceName: "keycloak",
                    realm: "eshop",
                    configureOptions: options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.Audience = "account";
                    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();

app.MapBasketEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();