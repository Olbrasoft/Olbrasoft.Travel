using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public class PointsOfInterestToRegionsFacade : TravelFacade<PointOfInterestToRegion>, IPointsOfInterestToRegionsFacade
    {

        protected new readonly IPointsOfInterestToRegionsRepository Repository;

        private IDictionary<int, int> _pointOfInterestIdsToRegionIds;
        private IDictionary<int, int> _regionIdsToPointOfInterestIds;
        public PointsOfInterestToRegionsFacade(IPointsOfInterestToRegionsRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public IDictionary<int, int> PointOfInterestIdsToRegionIds(bool clearFacadeCache = false)
        {
            if (_pointOfInterestIdsToRegionIds == null || clearFacadeCache)
                _pointOfInterestIdsToRegionIds =
                    Repository.GetAll().ToDictionary(k => k.Id, v => v.ToId);

            return _pointOfInterestIdsToRegionIds;
        }

        public IDictionary<int, int> RegionIdsToPointOfInterestIds(bool clearFacadeCache = false)
        {
            if (_regionIdsToPointOfInterestIds == null || clearFacadeCache)
                _regionIdsToPointOfInterestIds =
                    Repository.GetAll().ToDictionary(k => k.ToId, v => v.Id);

            return _regionIdsToPointOfInterestIds;
        }

        public void BulkSave(PointOfInterestToRegion[] pointsOfInterestToRegions)
        {
            Repository.BulkInsert(pointsOfInterestToRegions);
        }
    }
}