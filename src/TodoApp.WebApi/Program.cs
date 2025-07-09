using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TodoApp.ServiceDefaults;
using TodoApp.WebApi.Contexts;
using TodoApp.WebApi.Features.CreateTodo;
using TodoApp.WebApi.Features.DeleteTodo;
using TodoApp.WebApi.Features.GetTodo;
using TodoApp.WebApi.Features.UpdateTodo;

namespace TodoApp.WebApi
{
    public class Program
    {
        protected Program() { }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddServiceDefaults();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<TodoContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            var app = builder.Build();

            app.MapDefaultEndpoints();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            Seed(app);

            new CreateTodoEndpoint().Register(app);
            new GetTodoEndpoint().Register(app);
            new UpdateTodoEndpoint().Register(app);
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