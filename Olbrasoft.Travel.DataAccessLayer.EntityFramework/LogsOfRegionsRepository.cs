using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer.EntityFramework
{
    public class LogsOfRegionsRepository: BaseRepository<LogOfImport>,ILogsOfImportsRepository
    {
        public LogsOfRegionsRepository(TravelContext context) : base(context)
        {
        }
    }
}