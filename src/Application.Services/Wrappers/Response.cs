using System.Net;
using System.Text.Json.Serialization;

namespace Application.Services.Wrappers
{
    /// <summary>
    /// Representa una respuesta para el cliente
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Indica si la operación fue exitosa
        /// </summary>
        [JsonIgnore]
        public bool Succeeded { get; set; }

        /// <summary>
        /// Código de estatus HTTP
        /// </summary>
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Mensajes de error para el cliente
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> Errors { get; set; }

        /// <summary>
        /// Genera una respuesta exitosa
        /// </summary>
        /// <param name="statusCode">Código de estatus HTTP</param>
        /// <returns>Respuesta exitosa</returns>
        /// <exception cref="ArgumentException"></exception>
        public static Response Success(HttpStatusCode statusCode = HttpStatusCode.NoContent)
        {
            if (statusCode >= HttpStatusCode.BadRequest)
            {
                var message = "Valor inválido para una respuesta de éxito";
                throw new ArgumentException(message, nameof(statusCode));
            }

            return new()
            {
                Succeeded = true,
                StatusCode = statusCode,
            };
        }

        /// <summary>
        /// Genera una respuesta de error
        /// </summary>
        /// <param name="statusCode">Código de estatus HTTP</param>
        /// <param name="errors">Mensajes de error</param>
        /// <returns>Respuesta de error</returns>
        /// <exception cref="ArgumentException"></exception>
        public static Response Fail(HttpStatusCode statusCode, params string[] errors)
        {
            if (statusCode < HttpStatusCode.BadRequest)
            {
                var message = "Valor inválido para una respuesta de error";
                throw new ArgumentException(message, nameof(statusCode));
            }

            return new()
            {
                Succeeded = false,
                StatusCode = statusCode,
                Errors = errors
            };
        }
    }

    /// <summary>
    /// Representa una respuesta que devuelve un valor para el cliente
    /// </summary>
    /// <typeparam name="T">Tipo de dato a devolver</typeparam>
    public class Response<T> : Response
    {
        /// <summary>
        /// Valor devuelto
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T Value { get; set; }

        /// <summary>
        /// Genera una respuesta exitosa
        /// </summary>
        /// <param name="value">Valor a devolver</param>
        /// <param name="statusCode">Código de estatus HTTP</param>
        /// <returns>Respuesta exitosa con el valor indicado</returns>
        /// <exception cref="ArgumentException"></exception>
        public static Response<T> Success(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (statusCode >= HttpStatusCode.BadRequest)
            {
                var message = "Valor inválido para una respuesta de éxito";
                throw new ArgumentException(message, nameof(statusCode));
            }

            return new()
            {
                Succeeded = true,
                StatusCode = statusCode,
                Value = value
            };
        }

        /// <summary>
        /// Genera una respuesta de error
        /// </summary>
        /// <param name="statusCode">Código de estatus HTTP</param>
        /// <param name="errors">Mensajes de error</param>
        /// <returns>Respuesta de error</returns>
        /// <exception cref="ArgumentException"></exception>
        public static new Response<T> Fail(HttpStatusCode statusCode, params string[] errors)
        {
            if (statusCode < HttpStatusCode.BadRequest)
            {
                var message = "Valor inválido para una respuesta de error";
                throw new ArgumentException(message, nameof(statusCode));
            }

            return new()
            {
                Succeeded = false,
                StatusCode = statusCode,
                Errors = errors
            };
        }
    }
}
