using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DAL.EntityFramework;

namespace Olbrasoft.Travel.DataAccessLayer.EntityFramework
{
    public class LogsOfRegionsRepository: BaseRepository<LogOfImport>,ILogsOfImportsRepository
    {
        public LogsOfRegionsRepository(TravelContext context) : base(context)
        {
        }


      

       
    }
}