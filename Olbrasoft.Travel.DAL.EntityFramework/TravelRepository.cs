using System.Data.Entity;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class TravelRepository<T> :SharpRepository.EfRepository.EfRepository<T>, ITravelRepository<T> where T : class
    {
        protected new readonly TravelContext Context;

        public TravelRepository(TravelContext travelContext) : base(travelContext)
        {
            Context = travelContext;
        }
    }
    
}
