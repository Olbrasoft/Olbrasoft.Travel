using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class LocalizedRegionsRepository:LocalizedRepository<LocalizedRegion>,ILocalizedRegionsRepository
    {
        public LocalizedRegionsRepository(DbContext context) : base(context)
        {
        }
    }
}
