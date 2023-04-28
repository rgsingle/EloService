using EloForDumDums.Models;
using Microsoft.EntityFrameworkCore;

namespace EloForDumDums
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddAutoMapper(config => config.AddProfile<MapperProfile>());

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connstring = builder.Configuration.GetValue("Database", "inmemory");

            Console.WriteLine(connstring);

            if (connstring.ToLower() == "inmemory")
                builder.Services.AddDbContext<Context>(options => options.UseInMemoryDatabase("test.db"));
            else
                builder.Services.AddDbContext<Context>(options => options.UseNpgsql(connstring));
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}