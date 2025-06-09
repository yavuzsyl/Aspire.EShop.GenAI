var builder = DistributedApplication.CreateBuilder(args);

//projects & cloud-native backing services

//backing services

var postgres = builder
    .AddPostgres("postgres") //container
    .WithPgAdmin() //ui
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = postgres.AddDatabase(name: "catalogdb"); //only for referencing, we still need to create db 

var cache = builder
    .AddRedis("cache")
    .WithRedisInsight()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var rabbitmq = builder
    .AddRabbitMQ("rabbitmq")
    .WithManagementPlugin() // ui
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var keycloak = builder
    .AddKeycloak("keycloak", 8080)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

//projects

var catalog = builder
    .AddProject<Projects.Catalog>("catalog")
    .WithReference(catalogDb)//injects cs to environment
    .WithReference(rabbitmq)
    .WaitFor(catalogDb)
    .WaitFor(rabbitmq);

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
    .WithReference(catalog)
    .WithReference(basket)
    .WaitFor(catalog)
    .WaitFor(basket);



builder.Build().Run();
