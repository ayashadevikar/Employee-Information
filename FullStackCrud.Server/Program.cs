using FullStackCrud.Server.Data;
using FullStackCrud.Server.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FullStackCrud.Server.Helpers;

namespace FullStackCrud.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            // ✅ Configure MongoDB connection string (directly from environment variable)
            var mongoConnectionString = configuration["ConnectionStrings:Connection"];
            if (string.IsNullOrEmpty(mongoConnectionString))
            {
                throw new InvalidOperationException("MongoDB Atlas connection string not found in environment variables.");
            }

            // ✅ Register MongoClient as a Singleton
            builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));

            // ✅ Register services
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<EmployeeService>();

            // ✅ Add controllers and Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ✅ CORS setup (must match deployed frontend domain)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("https://employee-information-3f37.vercel.app")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // ✅ JWT authentication setup
            var jwtKey = configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT key not found in environment variables.");
            }

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

            // ✅ CORS middleware BEFORE routing
            app.UseCors("AllowSpecificOrigin");

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Swagger only in Development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            // API routes
            app.MapControllers();
            app.MapGet("/", () => "Backend is running!");
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
