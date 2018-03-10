using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public class AirportsFacade : TravelFacade<Airport>, IAirportsFacade
    {
        protected new readonly IAirportsRepository Repository;

        public AirportsFacade(IAirportsRepository repository) : base(repository)
        {
            Repository = repository;
        }

      
    }
}