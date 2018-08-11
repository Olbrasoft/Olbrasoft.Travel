using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DataAccessLayer;

namespace Olbrasoft.Travel.BusinessLogicLayer
{
    public class ImportsLogger :ILoggingImports
    {
        protected readonly ILogsOfImportsRepository Repository;

        public ImportsLogger(ILogsOfImportsRepository repository)
        {
            Repository = repository;
        }

        public void LogIn(LogOfImport log)
        {
            Repository.Add(log);
        }

        public void Log(string textForLogging)
        {
           LogIn(new LogOfImport(){Log = textForLogging});
        }
    }
}
