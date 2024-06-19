using Services.Wrappers;
using System.Net;
using System.Text.Json;

namespace WebAPI.Middlewares
{
    /// <summary>
    /// Manejador de errores
    /// </summary>
    /// <param name="requestDelegate"></param>
    /// <param name="logger"></param>
    public class ErrorHandlerMiddleware(RequestDelegate requestDelegate, ILogger<ErrorHandlerMiddleware> logger)
    {
        private readonly RequestDelegate _requestDelegate = requestDelegate;
        private readonly ILogger<ErrorHandlerMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context); // Ejecuta el siguiente método en el pipeline
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción no controlada");
                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.ContentType = "application/json";
                var responseModel = Result.Failed(HttpStatusCode.InternalServerError, ["Hubo un problema al procesar la solicitud"]);
                var json = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(json);
            }
        }
    }
}
