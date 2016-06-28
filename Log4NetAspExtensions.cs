using System.IO;
using Microsoft.AspNetCore.Hosting;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Logging;

namespace Log4net.Extensions.Logging
{
    public static class Log4NetAspExtensions
    {
        public static void ConfigureLog4Net(this IHostingEnvironment appEnv, string configFileRelativePath)
        {
            GlobalContext.Properties["appRoot"] = appEnv.ContentRootPath;
            XmlConfigurator.Configure(new FileInfo(Path.Combine(appEnv.ContentRootPath, configFileRelativePath)));
        }
        
        public static void ConfigureLog4Net(string currentDir, string configFileRelativePath)
        {
            GlobalContext.Properties["appRoot"] = currentDir;
            XmlConfigurator.Configure(new FileInfo(Path.Combine(currentDir, configFileRelativePath)));
        }

        public static void AddLog4Net(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new Log4NetProvider());
        }
    }
}
