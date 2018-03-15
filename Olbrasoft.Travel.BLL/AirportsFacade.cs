using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public class AirportsFacade : TravelFacade<Airport>, IAirportsFacade
    {
        protected new readonly IMapTo Repository;

        public AirportsFacade(IMapTo repository) : base(repository)
        {
            Repository = repository;
        }

      
    }
}