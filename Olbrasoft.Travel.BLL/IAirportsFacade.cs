using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public interface IAirportsFacade : ITravelFacade<Airport>
    {
        IDictionary<long, BaseAirport> EanAirportsIdsToBaseAirports();
        void BulkSave(Airport[] airports);
    }

    public class AirportsFacade : TravelFacade<Airport>, IAirportsFacade
    {
        protected new readonly IAirportsRepository Repository;

        public AirportsFacade(IAirportsRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public IDictionary<long, BaseAirport> EanAirportsIdsToBaseAirports()
        {
            return Repository.EanAirportsToBaseAirports();
        }

        public void BulkSave(Airport[] airports)
        {
            Repository.BulkInsert(airports.Where(airport => airport.Id == 0));
            Repository.BulkUpdate(airports.Where(airport => airport.Id != 0));
        }
    }
}