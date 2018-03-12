using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
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