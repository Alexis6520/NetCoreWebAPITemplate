using System.Net;
using System.Text.Json.Serialization;

namespace Services.Wrappers
{
    /// <summary>
    /// Representa el resultado de una operación solicitada por el cliente
    /// </summary>
    /// <param name="succeeded"></param>
    /// <param name="statusCode"></param>
    /// <param name="errors"></param>
    public class Result(bool succeeded, HttpStatusCode statusCode, IEnumerable<string>? errors = null)
    {
        /// <summary>
        /// Indica si la operación fue exitosa
        /// </summary>
        [JsonIgnore]
        public bool Succeeded { get; set; } = succeeded;

        /// <summary>
        /// Estatus a devolver al cliente
        /// </summary>
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; } = statusCode;

        /// <summary>
        /// Mensajes de error a mostrar al cliente
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string>? Errors { get; set; } = errors;

        /// <summary>
        /// Devuelve un resultado exitoso
        /// </summary>
        /// <param name="statusCode">Estatus a devolver</param>
        /// <returns></returns>
        public static Result Success(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (statusCode >= HttpStatusCode.BadRequest)
            {
                throw new ArgumentException("El valor proporcionado no representa un código de éxito", nameof(statusCode));
            }

            return new(true, statusCode);
        }

        /// <summary>
        /// Devuelve un resultado fallido
        /// </summary>
        /// <param name="statusCode">Estatus a devolver</param>
        /// <returns></returns>
        public static Result Failed(HttpStatusCode statusCode, IEnumerable<string> errors)
        {
            if (statusCode < HttpStatusCode.BadRequest)
            {
                throw new ArgumentException("El valor proporcionado no representa un código de error", nameof(statusCode));
            }

            return new(false, statusCode, errors);
        }
    }
}
