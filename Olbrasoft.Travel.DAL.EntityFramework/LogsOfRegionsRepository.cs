using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class LogsOfRegionsRepository: BaseRepository<LogOfImport>,ILogsOfImportsRepository
    {
        public LogsOfRegionsRepository(TravelContext context) : base(context)
        {
        }


      

       
    }
}