using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public interface IRegionsFacade 
    {
        
        IDictionary<long, int> GetMappingEanRegionIdsToIds(bool clearFacadeChache=false);

        IEnumerable<Region> Get(string typeOfRegionName);
        
        void Import(IEnumerable<Region> regions);
        


    }
}