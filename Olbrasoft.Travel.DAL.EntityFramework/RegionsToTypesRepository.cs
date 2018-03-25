using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class RegionsToTypesRepository : ManyToManyRepository<RegionToType>,IRegionsToTypesRepository
    {
        public RegionsToTypesRepository(DbContext context) : base(context)
        {
        }

        //public override void BulkSave(IEnumerable<RegionToType> regionsToTypes, params Expression<Func<RegionToType, object>>[] ignorePropertiesWhenUpdating)
        //{
        
        //    var forUpdate = new Queue<RegionToType>();

        //    var regionsToTypesArray = regionsToTypes as RegionToType[] ?? regionsToTypes.ToArray();
        //    foreach (var regionToType in regionsToTypesArray)
        //    {
        //        var tup = new Tuple<int, int>(regionToType.Id, regionToType.ToId);

        //        if (IdsToToIds.Contains(tup))
        //        {
        //            forUpdate.Enqueue(regionToType);
        //        }
                
        //    }

        //    Context.BulkUpdate(forUpdate,OnSaved,ignorePropertiesWhenUpdating);
            
        //    base.BulkSave(regionsToTypesArray, ignorePropertiesWhenUpdating);
            
        //}
    }
}