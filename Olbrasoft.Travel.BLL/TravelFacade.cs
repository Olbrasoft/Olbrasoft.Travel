using System.Collections.Generic;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public class TravelFacade<T> : ITravelFacade<T> where T : class
    {
        protected readonly IBaseRepository<T> Repository;

        public TravelFacade(IBaseRepository<T> repository)
        {
            Repository = repository;
        }

        public void Add(T item)
        {
            Repository.Add(item);
        }

        public void Add(IEnumerable<T> items)
        {
            Repository.Add(items);
        }

        public void Update(T item)
        {
            Repository.Update(item);
        }
    }
}
