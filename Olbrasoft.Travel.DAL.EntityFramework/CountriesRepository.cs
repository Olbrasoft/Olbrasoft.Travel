using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class CountriesRepository: TravelRepository<Country>, ICountriesRepository
    {
        private IReadOnlyDictionary<string, int> _codesToIds;

        public IReadOnlyDictionary<string, int> CodesToIds
        {
            get
            {
                return _codesToIds ??
                       (_codesToIds = GetAll(c => new {c.Code, c.Id}).ToDictionary(k => k.Code, v => v.Id));
            }

            private set => _codesToIds = value;
        }

        public CountriesRepository(DbContext context) : base(context)
        {

        }

        public override void ClearCache()
        {
            CodesToIds = null;
            base.ClearCache();
        }
    }
}