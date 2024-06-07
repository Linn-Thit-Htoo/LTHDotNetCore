using LTHDotNetCore.ConsoleApp;
using LTHDotNetCore.RestApi;

namespace LTHDotNetCore.MinimalApi
{
    public static class NLogServiceExtension
    {
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}
