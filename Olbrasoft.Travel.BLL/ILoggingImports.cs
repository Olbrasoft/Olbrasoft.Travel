using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public interface ILoggingImports
    {
        void LogIn(LogOfImport log);

        void Log(string textForLogging);
    }
    
}
