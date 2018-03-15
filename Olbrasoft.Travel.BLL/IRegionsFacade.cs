using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public interface IRegionsFacade : ITravelFacade<Region>
    {
        //HashSet<long> GetEanRegionsIds(bool clearFacadeCache = false);

        IDictionary<long, int> GetMappingEanRegionIdsToIds(bool clearFacadeCache = false);

        IDictionary<int, int> RegionIdsToParentRegionIds(bool clearFacadeCache = false);



        bool ExistsTypesOfRegions(Expression<Func<TypeOfRegion, bool>> predicate = null);

        HashSet<string> GetNamesOfTypesOfRegions(Expression<Func<TypeOfRegion, bool>> predicate = null);

        IDictionary<string, int> TypesOfRegionsAsReverseDictionary(bool clearFacadeCache = false);
        
        //IDictionary<long, Geo> GetMappingEanRegionIdsToRegions(bool clearFacadeCache=false);

       

    }
}