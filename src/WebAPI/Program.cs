using Infrastructure;
using Logic;
using NLog;
using NLog.Web;
using WebAPI;
using WebAPI.Middlewares;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
var builder = WebApplication.CreateBuilder(args);

try
{
    // Add services to the container.
    builder.Services
        .AddInfrastructure(builder.Configuration)
        .AddLogic()
        .AddSwagger()
        .AddAuthentication(builder.Configuration);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<NLogRequestPostedBodyMiddleware>(new NLogRequestPostedBodyMiddlewareOptions());
    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.InitializeDatabase();
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Programa detenido por excepcion");
    throw;
}
finally
{
    LogManager.Shutdown();
}

public partial class Program { }