using System;
using System.Linq.Expressions;
using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.BusinessLogicLayer
{
    public interface ITypesOfRegionsFacade:ITravelFacade<TypeOfRegion>
    {
        TypeOfRegion Get(string name, params Expression<Func<TypeOfRegion, object>>[] includeProperties);

    }
}
