using log4net.Config;
using log4net;

namespace LTHDotNetCoreMvcApp
{
    public static class Log4netExtensions
    {
        public static void AddLog4net(this IServiceCollection services)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            services.AddSingleton(LogManager.GetLogger(typeof(Program)));
        }
    }
}
