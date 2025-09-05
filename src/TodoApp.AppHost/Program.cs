namespace TodoApp.AppHost;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var postgres = builder.AddPostgres("postgres");
        var postgresDb = postgres.AddDatabase("Database", databaseName: "todoapp-db");

        var username = builder.AddParameter("username", "local");
        var password = builder.AddParameter("password", "local");
        var rabbitmq = builder.AddRabbitMQ("RabbitMQ", username, password)
            .WithManagementPlugin();

        builder.AddProject<Projects.TodoApp_WebApi>("todoapp-webapi")
            .WithReference(postgresDb)
            .WithReference(rabbitmq)
            .WaitFor(postgresDb)
            .WaitFor(rabbitmq);

        await builder.Build().RunAsync();
    }
}