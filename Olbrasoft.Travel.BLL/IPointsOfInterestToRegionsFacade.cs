using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public interface IPointsOfInterestToRegionsFacade : ITravelFacade<PointOfInterestToRegion>
    {
        IDictionary<int, int> PointOfInterestIdsToRegionIds(bool clearFacadeCache = false);

        IDictionary<int, int> RegionIdsToPointOfInterestIds(bool clearFacadeCache = false);

        void BulkSave(PointOfInterestToRegion[] pointsOfInterestToRegions);
    }
}