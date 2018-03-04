using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public interface ITypesOfRegionsFacade:ITravelFacade<TypeOfRegion>
    {
        TypeOfRegion Get(string name, params Expression<Func<TypeOfRegion, object>>[] includeProperties);

    }
}
