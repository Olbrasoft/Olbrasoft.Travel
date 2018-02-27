using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace Olbrasoft.Travel.EAN.Import
{
   
    public class ConsoleLogger : ILogger
    {
        protected readonly ILog Logger;

        public ConsoleLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            Logger = LogManager.GetLogger(typeof(EanImport));
        }

        public void Log(string message)
        {
            Logger.Info(message);
            //Console.WriteLine(message);
        }
    }
}