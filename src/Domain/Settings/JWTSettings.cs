namespace Domain.Settings
{
    /// <summary>
    /// Modela la configuración para la autenticación de JWT (Json Web Token)
    /// </summary>
    public class JWTSettings
    {
        /// <summary>
        /// Clave secreta de encriptado
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// IP/DNS del emisor del Token
        /// </summary>
        public string ValidIssuer { get; set; }

        /// <summary>
        /// Nombres de audiencias permitidas
        /// </summary>
        public IEnumerable<string> ValidAudiences { get; set; }
    }
}
