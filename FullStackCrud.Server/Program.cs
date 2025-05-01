using FullStackCrud.Server.Data;
using FullStackCrud.Server.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FullStackCrud.Server.Helpers;
using DotNetEnv;

namespace FullStackCrud.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Load .env file locally (in development)
            if (File.Exists(".env"))
            {
                Env.Load();
            }

            var builder = WebApplication.CreateBuilder(args);

            // Load sensitive config values from environment variables
            var mongoConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
            var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
            var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");

            if (string.IsNullOrEmpty(mongoConnectionString))
            {
                throw new Exception("DATABASE_CONNECTION_STRING environment variable is missing.");
            }

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new Exception("JWT_SECRET_KEY environment variable is missing.");
            }

            // Register MongoClient as Singleton
            builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                return new MongoClient(mongoConnectionString);
            });

            // Register services
            builder.Services.AddScoped<EmployeeService>();
            builder.Services.AddScoped<UserService>();

            // Manually configure DatabaseSettings from environment variables
            builder.Services.Configure<DatabaseSettings>(options =>
            {
                options.Connection = mongoConnectionString;
                options.DatabaseName = databaseName;
                options.EmployeeCollectionName = "employees";
                options.UserCollectionName = "users";
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // CORS setup (adjust origin as needed for production)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // JWT authentication setup
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "FullStackCrud.Server",
                    ValidAudience = "fullstackcrud.client",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.MapGet("/", () => "Backend is running!");
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
