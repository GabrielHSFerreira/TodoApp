var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var postgresDb = postgres.AddDatabase("Database", databaseName: "todoapp-db");

builder.AddProject<Projects.TodoApp_WebApi>("todoapp-webapi")
    .WithReference(postgresDb)
    .WaitFor(postgresDb);

await builder.Build().RunAsync();