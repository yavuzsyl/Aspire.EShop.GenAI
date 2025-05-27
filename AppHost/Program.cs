var builder = DistributedApplication.CreateBuilder(args);

//projects & cloud-native backing services

//backing services

var postgres = builder
    .AddPostgres("postgres") //container
    .WithPgAdmin() //ui
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = postgres.AddDatabase(name: "catalogdb"); //only for referencing, we still need to create db 

var redis = builder
    .AddRedis("cache")
    .WithRedisInsight()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

//projects

var catalog = builder
    .AddProject<Projects.Catalog>("catalog")
    .WithReference(catalogDb)//injects cs to environment
    .WaitFor(catalogDb);

var basket = builder
    .AddProject<Projects.Basket>("basket")
    .WithReference(redis)
    .WaitFor(redis);

builder.Build().Run();
