using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    //public class BaseRegionsRepository<T> : BaseRepository<T>, IBaseRegionsRepository<T> where T : Geo
    //{
    //    private IEnumerable<long> _eanRegionIds;
    //    private IReadOnlyDictionary<long, int> _eanRegionIdsToIds;

    //    public IEnumerable<long> EanRegionIds
    //    {
    //        get
    //        { 
    //            return _eanRegionIds ?? (_eanRegionIds = GetAll(p => p.EanId));
    //        }
    //    }

    //    public IReadOnlyDictionary<long, int> EanRegionIdsToIds
    //    {
    //        get
    //        {
    //            return _eanRegionIdsToIds ?? (_eanRegionIdsToIds =
    //                       FindAll(p => p.EanId >= 0, s => new { EanRegionId = s.EanId, s.Id })
    //                           .ToDictionary(k => k.EanRegionId, v => v.Id));
    //        }
    //    }

    //    private long _minEanRegionId = long.MinValue;

    //    private long MinEanRegionId
    //    {
    //        get
    //        {
    //            if (_minEanRegionId != long.MinValue) return _minEanRegionId;
    //            if (Exists(region => region.EanId < 0))
    //            {
    //                _minEanRegionId = Min(poi => poi.EanId) - 1;
    //            }
    //            else
    //            {
    //                _minEanRegionId = -1;
    //            }
    //            return _minEanRegionId;
    //        }
    //    }

    //    public BaseRegionsRepository(DbContext context) : base(context)
    //    {

    //    }


    //    public override void ClearCache()
    //    {
    //        _eanRegionIds = null;
    //        _eanRegionIdsToIds = null;
    //        _minEanRegionId = long.MinValue;

    //    }
        
    //    public new void Add(T baseRegion)
    //    {
    //        if (baseRegion.EanId == long.MinValue)
    //        {
    //            baseRegion.EanId = MinEanRegionId;
    //        }

    //        base.Add(baseRegion);
    //    }


    //    private IEnumerable<T> RebuildEanRegionIds(IEnumerable<T> baseRegions)
    //    {
    //        var regions = baseRegions as T[] ?? baseRegions.ToArray();

    //        foreach (var region in regions.Where(p => p.EanId >= 0 && p.Id == 0))
    //        {
    //            if (!EanRegionIdsToIds.TryGetValue(region.EanId, out var id)) continue;
    //            region.Id = id;
    //        }

    //        if (regions.All(p => p.EanId != long.MinValue)) return regions;
            
    //        foreach (var baseRegion in regions.Where(p => p.EanId == long.MinValue))
    //        {
    //            baseRegion.EanId = MinEanRegionId;
    //            _minEanRegionId = (_minEanRegionId - 1);
    //        }

    //        return regions;
    //    }

        
    //    public new void Add(IEnumerable<T> baseRegions)
    //    {
    //        base.Add(RebuildEanRegionIds(baseRegions));
    //    }

    //    public override void BulkSave(IEnumerable<T> baseRegions)
    //    {
    //        base.BulkSave(RebuildEanRegionIds(baseRegions));
    //    }
    //}
}
