using Domain.Settings;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Wrappers;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace WebAPI
{
    public static class Startup
    {
        /// <summary>
        /// Initializa la base de datos
        /// </summary>
        /// <param name="app"></param>
        public static void InitializeDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            using var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        }

        /// <summary>
        /// Agrega y configura el documentador de Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            return services;
        }

        /// <summary>
        /// Agrega la autenticación
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection(nameof(JWTSettings)).Get<JWTSettings>();
            _ = settings ?? throw new Exception("No se pudo obtener la configuración de autenticación");

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = BuildTokenValidationParameters(settings);
                x.Events = BuildJwtBearerEvents();
            });

            return services;
        }

        private static TokenValidationParameters BuildTokenValidationParameters(JWTSettings settings)
        {
            var key = Encoding.UTF8.GetBytes(settings.SecretKey);

            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = settings.ValidIssuer,
                ValidAudiences = settings.ValidAudiences,
            };
        }

        private static JwtBearerEvents BuildJwtBearerEvents()
        {
            return new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    var result = JsonSerializer.Serialize(Result.Failed(HttpStatusCode.Unauthorized, ["No autorizado"]));
                    return context.Response.WriteAsync(result);
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    var result = JsonSerializer.Serialize(Result.Failed(HttpStatusCode.Forbidden, ["No tiene acceso a este recurso"]));
                    return context.Response.WriteAsync(result);
                }
            };
        }
    }
}
