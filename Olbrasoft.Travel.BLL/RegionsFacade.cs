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

        private IDictionary<long, int> _mappingEanRegionIdsToIds;
        private IDictionary<string, int> _typesOfRegionsAsReverseDictionary;
        private IDictionary<string, int> _subClassesOfRegionsAsReverseDictionary;
        private IDictionary<long, int> _mappingPointsOfInterestEanRegionIdToIds;

        public RegionsFacade(ITravelRepository<Region> repository, ITypesOfRegionsRepository typesOfRegionsRepository, ISubClassesRepository subClassesRepository, IPointsOfInterestRepository pointsOfInterestRepository) : base(repository)
        {
            TypesOfRegionsRepository = typesOfRegionsRepository;
            SubClassesRepository = subClassesRepository;
            PointsOfInterestRepository = pointsOfInterestRepository;
        }
        

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

        public IDictionary<long, int> GetMappingPointsOfInterestEanRegionIdsToIds(bool clearFacadeCache = false)
        {
            if (_mappingPointsOfInterestEanRegionIdToIds == null || clearFacadeCache)
            {
                _mappingPointsOfInterestEanRegionIdToIds = PointsOfInterestRepository.AsQueryable()
                    .Where(p => p.EanRegionId != null)
                    .Select(p => new { EanRegionId = (long)p.EanRegionId, p.Id }).ToDictionary(key => key.EanRegionId, val => val.Id);
            }
            return _mappingPointsOfInterestEanRegionIdToIds;
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

        public IDictionary<string, int> SubClassesAsReverseDictionary(bool clearFacadeCache = false)
        {
            if (_subClassesOfRegionsAsReverseDictionary == null || clearFacadeCache)
            {
                _subClassesOfRegionsAsReverseDictionary =
                    SubClassesRepository.GetAll(t => new { t.Name, t.Id }).ToDictionary(key => key.Name, val => val.Id);
            }

            return _subClassesOfRegionsAsReverseDictionary;
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

        public void Save(HashSet<SubClass> subClasses)
        {
            var storedNamesOfSubClasses = new HashSet<string>(SubClassesRepository.GetAll(s => s.Name));
            var subClassesToSave = new HashSet<SubClass>();

            foreach (var subClass in subClasses)
            {
                if (!storedNamesOfSubClasses.Contains(subClass.Name))
                {
                    subClassesToSave.Add(subClass);
                }
            }

            SubClassesRepository.Add(subClassesToSave);
        }
    }
}