using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public interface IPointsOfInterestFacade : ITravelFacade<PointOfInterest>
    {
        IDictionary<long, int> GetMappingEanRegionIdsToIds(bool clearFacadeCache = false);

        IDictionary<long, BasePointOfInterest> EanRegionIdsToBasePointsOfInterest(bool clearCache = false);

        IDictionary<int, int> PointOfInterestIdsToParentPointOfInterestIds(bool clearFacadeCache = false);

       

     
    }
}
