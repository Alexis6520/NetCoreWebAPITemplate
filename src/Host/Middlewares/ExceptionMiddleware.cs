using Application.Services.Wrappers;
using System.Net;
using System.Text.Json;

namespace Host.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, ILogger<ExceptionMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error interno al procesar solicitud");
                var responseModel = Response.Fail(HttpStatusCode.InternalServerError, "Hubo un problema al procesar su solicitud");
                var response = context.Response;
                response.StatusCode = (int)responseModel.StatusCode;
                response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(json);
            }
        }
    }
}
