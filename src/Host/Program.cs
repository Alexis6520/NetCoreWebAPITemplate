using Application.Services;
using Host.Extensions;
using Host.Middlewares;
using Infrastructure;
using NLog;
using NLog.Web;

var logger = LogManager.GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddApplicationServices()
        .AddInfrastructure();

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddCustomSwagger();

    var app = builder.Build();

    if (app.Environment.IsDevelopment() || app.Environment.IsTesting())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.InitializeDatabase();
    }

    app.UseMiddleware<ExceptionMiddleware>();
    app.UseHttpsRedirection();
    app.UseAuthorization();

    // needed for  ${aspnet-request-posted-body} with an API Controller.
    app.UseMiddleware<NLogRequestPostedBodyMiddleware>(
        new NLogRequestPostedBodyMiddlewareOptions());

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    logger.Fatal(ex, "Programa detenido por excepción");
    throw;
}
finally
{
    LogManager.Shutdown();
}

public partial class Program { }