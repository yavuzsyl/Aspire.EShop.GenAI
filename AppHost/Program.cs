var builder = DistributedApplication.CreateBuilder(args);

//projects & cloud-native backing services


//backing services
var postgres = builder
    .AddPostgres("postgres") //container
    .WithPgAdmin() //ui
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = postgres.AddDatabase(name: "catalogdb"); //only for referencing, we still need to create db 

//projects
var catalog = builder
    .AddProject<Projects.Catalog>("catalog")
    .WithReference(catalogDb)//injects cs to environment
    .WaitFor(catalogDb);


builder.Build().Run();
