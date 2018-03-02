using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public interface IRegionsFacade : ITravelFacade<Region>
    {
        IDictionary<long, int> GetMappingEanRegionIdsToIds(bool clearFacadeCache = false);

        IDictionary<long, int> GetMappingPointsOfInterestEanRegionIdsToIds(bool clearFacadeCache = false);

        //IEnumerable<Region> Get(string typeOfRegionName);
        //void Import(IEnumerable<Region> regions);

        bool ExistsTypesOfRegions(Expression<Func<TypeOfRegion, bool>> predicate = null);

        HashSet<string> GetNamesOfTypesOfRegions(Expression<Func<TypeOfRegion, bool>> predicate = null);

        IDictionary<string, int> TypesOfRegionsAsReverseDictionary(bool clearFacadeCache = false);

        IDictionary<string, int> SubClassesAsReverseDictionary(bool clearFacadeCache = false);

        void Save(HashSet<TypeOfRegion> typesOfRegions);

        void Save(HashSet<SubClass> subClasses);
    }
}