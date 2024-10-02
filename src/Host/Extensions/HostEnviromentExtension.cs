namespace Host.Extensions
{
    public static class HostEnviromentExtension
    {
        public static bool IsTesting(this IHostEnvironment enviroment) => enviroment.EnvironmentName == "Testing";
    }
}
