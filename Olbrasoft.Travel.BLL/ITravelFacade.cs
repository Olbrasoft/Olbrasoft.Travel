using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public interface ITravelFacade<T>where T:class
    {
        void Add(T item);
        void Add(IEnumerable<T> items);
        void Update(T item);

    }
}
