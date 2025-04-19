
//using Employee.Controllers;
using FullStackCrud.Server.Data;
//using FullStackCrud.Server.Models;
using FullStackCrud.Server.Services;  
//using Microsoft.Extensions.Options;


namespace FullStackCrud.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.Configure<DatabaseSettings>(
     builder.Configuration.GetSection("ConnectionStrings"));


  
            builder.Services.AddSingleton<EmployeeService>();

            builder.Services.AddControllers();

            // Swagger/OpenAPI setup
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Swagger UI only in development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapGet("/", () => "Backend is running!");
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }

};
