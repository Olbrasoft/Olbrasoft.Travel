using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{

    public class RegionsFacade : TravelFacade<Region>, IRegionsFacade
    {
        protected readonly ITypesOfRegionsRepository TypesOfRegionsRepository;
        protected readonly ISubClassesRepository SubClassesRepository;
        protected readonly IPointsOfInterestRepository PointsOfInterestRepository;
        protected readonly IRegionsToRegionsRepository RegionsToRegionsRepository;
        protected readonly IPointsOfInterestToRegionsFacade PointsOfInterestToRegionsFacade;

        private HashSet<long> _eanRegionsIds;
        private IDictionary<long, int> _mappingEanRegionIdsToIds;
        private IDictionary<string, int> _typesOfRegionsAsReverseDictionary;
        private IDictionary<int, int> _regionIdsToParentRegionIds;
       
        private IDictionary<long, BaseRegion> _mappingEanRegionIdsToRegions;

        public RegionsFacade(ITravelRepository<Region> repository, ITypesOfRegionsRepository typesOfRegionsRepository, ISubClassesRepository subClassesRepository, IPointsOfInterestRepository pointsOfInterestRepository, IRegionsToRegionsRepository regionsToRegionsRepository, IPointsOfInterestToRegionsFacade pointsOfInterestToRegionsFacade) : base(repository)
        {
            TypesOfRegionsRepository = typesOfRegionsRepository;
            SubClassesRepository = subClassesRepository;
            PointsOfInterestRepository = pointsOfInterestRepository;
            RegionsToRegionsRepository = regionsToRegionsRepository;
            PointsOfInterestToRegionsFacade = pointsOfInterestToRegionsFacade;
        }

        //public HashSet<long> GetEanRegionsIds(bool clearFacadeCache = false)
        //{
        //    if (_eanRegionsIds == null || clearFacadeCache)
        //    {
        //        _eanRegionsIds = new HashSet<long>(Repository.AsQueryable()
        //            .Where(region => region.EanRegionId != null).Select(region => (long)region.EanRegionId));
        //    }

        //    return _eanRegionsIds;
        //}

        public IDictionary<long, int> GetMappingEanRegionIdsToIds(bool clearFacadeCache = false)
        {
            if (_mappingEanRegionIdsToIds == null || clearFacadeCache)
            {
                _mappingEanRegionIdsToIds = Repository.AsQueryable()
                    .Where(p => p.EanRegionId != null)
                    .Select(p => new { EanRegionId = (long)p.EanRegionId, p.Id }).ToDictionary(key => key.EanRegionId, val => val.Id);
            }
            return _mappingEanRegionIdsToIds;
        }

        public IDictionary<int, int> RegionIdsToParentRegionIds(bool clearFacadeCache = false)
        {
            if (_regionIdsToParentRegionIds == null || clearFacadeCache)
                _regionIdsToParentRegionIds = RegionsToRegionsRepository.GetAll()
                    .ToDictionary(k => k.Id, v => v.ToId);

            return _regionIdsToParentRegionIds;
        }

        public IDictionary<int, int> RegionIdsToPointOfInterestIds(bool clearFacadeCache = false)
        {
            return PointsOfInterestToRegionsFacade.RegionIdsToPointOfInterestIds();
        }


        public bool ExistsTypesOfRegions(Expression<Func<TypeOfRegion, bool>> predicate)
        {
            return TypesOfRegionsRepository.Exists(predicate);
        }

        public HashSet<string> GetNamesOfTypesOfRegions(Expression<Func<TypeOfRegion, bool>> predicate = null)
        {
            return new HashSet<string>(TypesOfRegionsRepository.FindAll(predicate).Select(t => t.Name));
        }

        public IDictionary<string, int> TypesOfRegionsAsReverseDictionary(bool clearFacadeCache = false)
        {

            if (_typesOfRegionsAsReverseDictionary == null || clearFacadeCache)
            {
                _typesOfRegionsAsReverseDictionary =
                    TypesOfRegionsRepository.GetAll(t => new { t.Name, t.Id }).ToDictionary(key => key.Name, val => val.Id);
            }

            return _typesOfRegionsAsReverseDictionary;
        }

       

        //public IDictionary<long, BaseRegion> GetMappingEanRegionIdsToRegions(bool clearFacadeCache = false)
        //{
        //    if (_mappingEanRegionIdsToRegions == null || clearFacadeCache)
        //    {
        //        _mappingEanRegionIdsToRegions = Repository.AsQueryable()
        //            .Where(region=>region.EanRegionId!=null).ToDictionary(k =>
        //            {
        //                if (k.EanRegionId != null) return (long) k.EanRegionId;
        //                return 0;
        //            }, v => new BaseRegion
        //            {
        //                Id = v.Id,
        //                CreatorId = v.CreatorId,
        //                EanRegionId = v.EanRegionId,
        //                TypeOfRegionId = v.TypeOfRegionId,
        //                DateAndTimeOfCreation = v.DateAndTimeOfCreation,
        //                SubClassId = v.SubClassId
        //            });
        //    }
        //    return _mappingEanRegionIdsToRegions;
        //}


        public void BulkSave(IEnumerable<Region> regions)
        {
            var regionsArray = regions as Region[] ?? regions.ToArray();
            Repository.BulkInsert(regionsArray.Where(region => region.Id == 0));
            Repository.BulkUpdate(regionsArray.Where(region => region.Id != 0));
        }

        public void Save(HashSet<TypeOfRegion> typesOfRegions)
        {
            var storedNamesOfTypesOfRegions = new HashSet<string>(TypesOfRegionsRepository.GetAll(t => t.Name));

            var typesOfRegionsToSave = new HashSet<TypeOfRegion>();

            foreach (var typeOfRegion in typesOfRegions)
            {
                if (!storedNamesOfTypesOfRegions.Contains(typeOfRegion.Name))
                {
                    typesOfRegionsToSave.Add(typeOfRegion);
                }
            }

            TypesOfRegionsRepository.Add(typesOfRegionsToSave);
        }

       

        public void BulkSave(PointOfInterestToRegion[] pointsOfInterestToRegions)
        {
            PointsOfInterestToRegionsFacade.BulkSave(pointsOfInterestToRegions);
        }
    }
}