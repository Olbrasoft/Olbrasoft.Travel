using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IPhotosOfAccommodationsRepository : IBulkRepository<PhotoOfAccommodation>,
        IBaseRepository<PhotoOfAccommodation>
    {
        IReadOnlyDictionary<Tuple<int, string, int>, int> GetPathIdsAndFileIdsAndExtensionIdsToIds();

        //IReadOnlyDictionary<Tuple<int, string, int>, int> GetPathIdsAndFileIdsAndExtensionIdsToIds(
        //    IEnumerable<int> pathIds);
    }
}