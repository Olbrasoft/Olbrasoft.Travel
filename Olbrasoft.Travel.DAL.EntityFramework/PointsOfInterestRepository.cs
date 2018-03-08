using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class PointsOfInterestRepository : KeyIdRepository<PointOfInterest>, IPointsOfInterestRepository
    {
        private IDictionary<long, BasePointOfInterest> _eanRegionIdsToBasePointsOfInterest;


        private long _minEanRegionId = long.MinValue;

        private long MinEanRegionId
        {
            get
            {
                if (_minEanRegionId != long.MinValue) return _minEanRegionId;
                if (Exists(region => region.EanRegionId < 0))
                {
                    _minEanRegionId = Min(poi => poi.EanRegionId) - 1;
                }
                else
                {
                    _minEanRegionId = -1;
                }
                return _minEanRegionId;
            }

        }
        
        public PointsOfInterestRepository(TravelContext travelContext) : base(travelContext)
        {
            OnSaved += ClearCache;
        }

        private void ClearCache(object sender, EventArgs eventArgs)
        {
            _eanRegionIdsToBasePointsOfInterest = null;
            _minEanRegionId = long.MinValue;
            ClearCache();
        }


        public new void Add(PointOfInterest pointOfInterest)
        {
            if (pointOfInterest.EanRegionId == long.MinValue)
            {
                pointOfInterest.EanRegionId = MinEanRegionId;
            }

            base.Add(pointOfInterest);
        }

        private IEnumerable<PointOfInterest> Rebuild(PointOfInterest[] pointsOfInterest)
        {
            if (pointsOfInterest.All(p => p.EanRegionId != long.MinValue)) return pointsOfInterest;

            foreach (var pointOfInterest in pointsOfInterest.Where(p => p.EanRegionId == long.MinValue))
            {
                pointOfInterest.EanRegionId = MinEanRegionId;
                _minEanRegionId = (_minEanRegionId - 1);
            }

            return pointsOfInterest;
        }
        

        public new void Add(IEnumerable<PointOfInterest> pointsOfInterest)
        {
            base.Add(Rebuild(pointsOfInterest.ToArray()));
        }
        
        public new void BulkInsert(IEnumerable<PointOfInterest> pointsOfInterest)
        {
            base.BulkInsert(Rebuild(pointsOfInterest.ToArray()));
        }
        
        public new void BulkUpdate(IEnumerable<PointOfInterest> pointsOfInterest)
        {
            base.BulkUpdate(Rebuild(pointsOfInterest.ToArray()));
        }
        public new void BulkInsertOrUpdate(PointOfInterest[] pointsOfInterest)
        {
            base.BulkInsertOrUpdate(Rebuild(pointsOfInterest.ToArray()).ToArray());
        }

        public IDictionary<long, BasePointOfInterest> EanRegionIdsToBasePointsOfInterest(bool clearCache = false)
        {

            if (_eanRegionIdsToBasePointsOfInterest == null || clearCache)
            {
                _eanRegionIdsToBasePointsOfInterest = AsQueryable()
                    .Where(poi => true).ToDictionary(poi => poi.EanRegionId, br => new BasePointOfInterest
                    {
                        Id = br.Id,
                        Shadow = br.Shadow,
                        EanRegionId = br.EanRegionId,
                        SubClassId = br.SubClassId,
                        CreatorId = br.CreatorId,
                        DateAndTimeOfCreation = br.DateAndTimeOfCreation
                    });
            }
            return _eanRegionIdsToBasePointsOfInterest;
        }


        
    }
}
