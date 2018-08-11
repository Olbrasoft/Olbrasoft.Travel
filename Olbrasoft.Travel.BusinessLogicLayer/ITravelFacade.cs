using System.Collections.Generic;

namespace Olbrasoft.Travel.BusinessLogicLayer
{
    public interface ITravelFacade<T>where T:class
    {
        void Add(T item);
        void Add(IEnumerable<T> items);
        void Update(T item);

    }
}
