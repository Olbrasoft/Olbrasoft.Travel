using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class KeyIdRepository<T>:TravelRepository<T> ,IKeyIdRepository<T> where T:class, IKeyId 
    {
        public KeyIdRepository(TravelContext travelContext) : base(travelContext)
        {
        }

        public void BulkSave(IEnumerable<T> entities)
        {
            var enumerable = entities as T[] ?? entities.ToArray();
            BulkInsert(enumerable.Where(e=>e.Id==0));
            BulkUpdate(enumerable.Where(e=>e.Id!=0));
        }
    }
}
