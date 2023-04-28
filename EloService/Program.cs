using EloService.Models;
using Microsoft.EntityFrameworkCore;

namespace EloService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Add automapper because lazy
            builder.Services.AddAutoMapper(config => config.AddProfile<MapperProfile>());

            // Add Healthcheck Endpoint
            builder.Services.AddHealthChecks();

            // Add Swagger
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
            }

            // Add Database Context (Postgres or Inmemory)
            var connstring = builder.Configuration.GetValue("Database", "inmemory");

            Console.WriteLine(connstring);

            if (connstring.ToLower() == "inmemory")
                builder.Services.AddDbContext<Context>(options => options.UseInMemoryDatabase("test.db"));
            else
                builder.Services.AddDbContext<Context>(options => options.UseNpgsql(connstring));
            
            // Build app
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapHealthChecks("/healthz");
            app.MapControllers();

            app.Run();
        }
    }
}