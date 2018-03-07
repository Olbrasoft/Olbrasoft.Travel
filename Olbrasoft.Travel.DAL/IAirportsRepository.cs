using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IAirportsRepository : ITravelRepository<Airport>
    {
        IDictionary<long, BaseAirport> EanAirportsToBaseAirports();
    }
}