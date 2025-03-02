using Microsoft.EntityFrameworkCore;
using TodoIntegrationTests.WebApi.Contexts;

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
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<TodoContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
            builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Program).Assembly));

            var app = builder.Build();

            app.MapDefaultEndpoints();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();

            Seed(app);

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