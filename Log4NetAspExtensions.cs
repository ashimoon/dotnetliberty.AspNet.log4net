using log4net;
using log4net.Config;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Logging;
using System.IO;

namespace dotnetliberty.AspNet.log4net
{
    public static class Log4NetAspExtensions
    {
        public static void ConfigureLog4Net(this IApplicationEnvironment appEnv, string configFileRelativePath)
        {
            GlobalContext.Properties["appRoot"] = appEnv.ApplicationBasePath;
            XmlConfigurator.Configure(new FileInfo(Path.Combine(appEnv.ApplicationBasePath, configFileRelativePath)));
        }

        public static void AddLog4Net(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new Log4NetProvider());
        }
    }
}
