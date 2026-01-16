using System.Text;
using GeoAssetManagementSystem.Interfaces;
using GeoAssetManagementSystem.Repositories;
using GeoAssetManagementSystem.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GeoAssetManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Database Connection
            builder.Services.AddDbContext<LocationContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));

            // 2. Identity Service
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<LocationContext>()
                .AddDefaultTokenProviders();

            // 3. Register Repository for Dependency Injection
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();

            // 4. JWT Authentication Setup
            // Fallback key ensures the app doesn't crash with a 500 error if appsettings is missing the key
            var jwtKey = builder.Configuration["JwtSettings:Key"] ?? "GeoAssetSystem_Secure_Key_2026_@_Secure_Long_String";
            var keyBytes = Encoding.ASCII.GetBytes(jwtKey);

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Add this after AddAuthentication
            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAngular", policy =>
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by a space and your JWT token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAngular");

            // 5. Correct Middleware Order
            app.UseAuthentication(); 
            app.UseAuthorization();  

            app.MapControllers();

            app.Run();
        }
    }
}
