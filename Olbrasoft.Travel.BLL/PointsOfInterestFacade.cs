using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public class PointsOfInterestFacade : TravelFacade<PointOfInterest>, IPointsOfInterestFacade
    {
        protected new readonly IPointsOfInterestRepository Repository;
        protected readonly IPointsOfInterestToPointsOfInterestRepository PointsOfInterestToPointsOfInterestRepository;
        protected readonly IPointsOfInterestToRegionsFacade PointsOfInterestToRegionsFacade;

        private IDictionary<long, int> _mappingEanRegionIdToIds;
        private IDictionary<int, int> _pointOfInterestIdsToParentPointOfInterestIds;

        public PointsOfInterestFacade(IPointsOfInterestRepository repository, IPointsOfInterestToPointsOfInterestRepository pointsOfInterestToPointsOfInterestRepository, IPointsOfInterestToRegionsFacade pointsOfInterestToRegionsFacade) : base(repository)
        {
            Repository = repository;
            PointsOfInterestToPointsOfInterestRepository = pointsOfInterestToPointsOfInterestRepository;
            PointsOfInterestToRegionsFacade = pointsOfInterestToRegionsFacade;
        }
        
        public IDictionary<long, int> GetMappingEanRegionIdsToIds(bool clearFacadeCache = false)
        {
            if (_mappingEanRegionIdToIds == null || clearFacadeCache)
            {
                _mappingEanRegionIdToIds = Repository.AsQueryable()
                    .Where(p => p.EanRegionId != null)
                    .Select(p => new { EanRegionId = (long)p.EanRegionId, p.Id }).ToDictionary(key => key.EanRegionId, val => val.Id);
            }
            return _mappingEanRegionIdToIds;
        }

        public IDictionary<long, BasePointOfInterest> EanRegionIdsToBasePointsOfInterest(bool clearFacadeCache = false)
        {
            return Repository.EanRegionIdsToBasePointsOfInterest();
        }

        public IDictionary<int, int> PointOfInterestIdsToParentPointOfInterestIds(bool clearFacadeCache = false)
        {
            if (_pointOfInterestIdsToParentPointOfInterestIds == null || clearFacadeCache)
                _pointOfInterestIdsToParentPointOfInterestIds = PointsOfInterestToPointsOfInterestRepository.GetAll()
                    .ToDictionary(k => k.Id, v => v.ToId);

            return _pointOfInterestIdsToParentPointOfInterestIds;
        }

        public IDictionary<int, int> PointOfInterestIdsToRegionIds(bool clearFacadeCache = false)
        {
            return PointsOfInterestToRegionsFacade.PointOfInterestIdsToRegionIds();
        }

        public void BulkSave(IEnumerable<PointOfInterest> pointsOfInterest)
        {
            var regionsArray = pointsOfInterest as PointOfInterest[] ?? pointsOfInterest.ToArray();
            Repository.BulkInsert(regionsArray.Where(pointOfInterest => pointOfInterest.Id == 0));
            Repository.BulkUpdate(regionsArray.Where(pointOfInterest => pointOfInterest.Id != 0));
        }

        public void BulkSave(PointOfInterestToPointOfInterest[] pointsOfInterestToPointsOfInterest)
        {
            PointsOfInterestToPointsOfInterestRepository.BulkInsert(pointsOfInterestToPointsOfInterest);
        }

        public void BulkSave(PointOfInterestToRegion[] pointsOfInterestToRegions)
        {
            PointsOfInterestToRegionsFacade.BulkSave(pointsOfInterestToRegions);
        }
    }
}