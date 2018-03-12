using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface ILoggingImports
    {
        void LogIn(LogOfImport log);

        void Log(string textForLogging);
    }
    
}
