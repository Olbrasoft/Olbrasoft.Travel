using System;
using System.Collections.Generic;
using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer
{
    public interface IPhotosOfAccommodationsRepository : IBulkRepository<PhotoOfAccommodation>,
        IBaseRepository<PhotoOfAccommodation>
    {
        IReadOnlyDictionary<Tuple<int, string, int>, int> GetPathIdsAndFileIdsAndExtensionIdsToIds();

        //IReadOnlyDictionary<Tuple<int, string, int>, int> GetPathIdsAndFileIdsAndExtensionIdsToIds(
        //    IEnumerable<int> pathIds);
    }
}