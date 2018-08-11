using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer
{
    public interface ILoggingImports
    {
        void LogIn(LogOfImport log);

        void Log(string textForLogging);
    }
    
}
