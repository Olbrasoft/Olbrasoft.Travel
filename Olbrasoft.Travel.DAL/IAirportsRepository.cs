using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IAirportsRepository : IBaseRepository<Airport>
    {
        IDictionary<long, BaseAirport> EanAirportsToBaseAirports();
    }
}