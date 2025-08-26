using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume()
    .AddDatabase("tododb");

var migrations = builder.AddProject<MigrationService>("migrations")
    .WithEnvironment("ConnectionStrings__Database", postgres)
    .WithReference(postgres)
    .WaitFor(postgres);

builder.AddProject<Web_Api>("api")
    .WithEnvironment("ConnectionStrings__Database", postgres)
    .WithReference(postgres)
    .WaitFor(postgres)
    .WithReference(migrations)
    .WaitForCompletion(migrations);

await builder.Build().RunAsync();
