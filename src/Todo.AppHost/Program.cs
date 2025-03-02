var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var postgresDb = postgres.AddDatabase("Database");

builder.AddProject<Projects.Todo_WebApi>("todo-webapi")
    .WithReference(postgresDb)
    .WaitFor(postgresDb);

await builder.Build().RunAsync();