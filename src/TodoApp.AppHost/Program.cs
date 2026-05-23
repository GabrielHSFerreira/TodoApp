namespace TodoApp.AppHost;

public static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var postgres = builder.AddPostgres("postgres")
            .WithImage("postgres", "18.4-alpine")
            .AddDatabase("Database", databaseName: "todoapp-db");

        builder.AddProject<Projects.TodoApp_WebApi>("todoapp-webapi")
            .WithReference(postgres)
            .WaitFor(postgres);

        await builder.Build().RunAsync();
    }
}