using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TodoIntegrationTests.WebApi.Contexts;
using TodoIntegrationTests.WebApi.Features.DeleteTodo;

namespace TodoIntegrationTests.WebApi
{
    public class Program
    {
        protected Program() { }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddServiceDefaults();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<TodoContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
            builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Program).Assembly));

            var app = builder.Build();

            app.MapDefaultEndpoints();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseAuthorization();
            app.MapControllers();

            Seed(app);

            new DeleteTodoEndpoint().Register(app);

            app.Run();
        }

        private static void Seed(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TodoContext>();

            context.Database.EnsureCreated();
        }
    }
}