using System.Data.Entity;
using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer.EntityFramework
{
    public class RegionsToTypesRepository : ManyToManyRepository<RegionToType>,IRegionsToTypesRepository
    {
        public RegionsToTypesRepository(DbContext context) : base(context)
        {
        }
        
    }
}