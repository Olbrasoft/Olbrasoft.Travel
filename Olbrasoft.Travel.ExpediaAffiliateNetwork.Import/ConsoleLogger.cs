using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DataAccessLayer;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.Import
{
    public class ConsoleLogger : ILogger ,ILoggingImports 
    {
        protected readonly ILog Logger;

        public ConsoleLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            
            Logger = LogManager.GetLogger(typeof(EanImport));
        }

        public void LogIn(LogOfImport log)
        {
           Log(log.Log);
        }

        public void Log(string message)
        {
            Logger.Info(message);
            //Console.WriteLine(message);
        }
    }
}