using Domain.Settings;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Infrastructure.Services.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Services;
using Services.Queries;
using System.Text.Json;
using System.Text;
using Services.Wrappers;
using System.Net;

namespace Infrastructure
{
    public static class ServiceExtension
    {
        /// <summary>
        /// Agrega las implementaciones de los servicios
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opts =>
            {
                opts.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(IUnitOfWork).Assembly);
            services.AddTransient<IDonutQueryService, DonutQueryService>();
            services.AddAuthentication(configuration);
            return services;
        }

        private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
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
