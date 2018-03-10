using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IPointsOfInterestRepository : IBaseRepository<PointOfInterest>
    {
        IDictionary<long, BasePointOfInterest> EanRegionIdsToBasePointsOfInterest(bool clearCache = false);
    }
}
