
using FullStackCrud.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;

namespace FullStackCrud.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<EmployeeContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("CRUDCS")));




            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors();

            builder.Services.AddControllers();

            var app = builder.Build();

            // Set static file path to ClientApp/dist
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp", "dist");

            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = new PhysicalFileProvider(rootPath)
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(rootPath)
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapGet("/", () => "Backend is running!");

            app.MapControllers();

            // Fallback to React index.html
            app.MapFallbackToFile("/index.html", new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(rootPath)
            });

            app.Run();
        }
    }
}
