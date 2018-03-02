using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
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
