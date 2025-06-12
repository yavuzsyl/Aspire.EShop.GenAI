var builder = DistributedApplication.CreateBuilder(args);

//projects & cloud-native backing services

//backing services

var postgres = builder
    .AddPostgres("postgres") //container
    .WithPgAdmin() //ui
    //.WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = postgres.AddDatabase(name: "catalogdb"); //only for referencing, we still need to create db 

var cache = builder
    .AddRedis("cache")
    .WithRedisInsight()
    //.WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var rabbitmq = builder
    .AddRabbitMQ("rabbitmq")
    .WithManagementPlugin() // ui
    //.WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var keycloak = builder
    .AddKeycloak("keycloak", 8080)
    //.WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

if (builder.ExecutionContext.IsRunMode)
{
    // Data volumes dont work on ACA for Postgres  https://github.com/dotnet/aspire/issues/6671
    // use cloud services in production
    postgres.WithDataVolume();
    cache.WithDataVolume();
    rabbitmq.WithDataVolume();
    keycloak.WithDataVolume();
}

var ollama = builder
    .AddOllama("ollama", 11435)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithOpenWebUI();

var llama = ollama.AddModel("llama3.2");

//projects

var catalog = builder
    .AddProject<Projects.Catalog>("catalog")
    .WithReference(catalogDb)//injects cs to environment
    .WithReference(rabbitmq)
    .WithReference(llama)
    .WaitFor(catalogDb)
    .WaitFor(rabbitmq)
    .WaitFor(llama);

var basket = builder
    .AddProject<Projects.Basket>("basket")
    .WithReference(cache) // connection string for redis
    .WithReference(catalog) // for service discovery http+https://localhost:xxxx
    .WithReference(rabbitmq)
    .WithReference(keycloak)
    .WaitFor(cache)
    .WaitFor(rabbitmq)
    .WaitFor(keycloak);


builder.AddProject<Projects.WebApp>("webapp")
    .WithExternalHttpEndpoints() // expose the app 
    .WithReference(cache)
    .WithReference(catalog)
    .WithReference(basket)
    .WaitFor(catalog)
    .WaitFor(basket);



builder.Build().Run();
