using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public interface ISubClassesFacade : ITravelFacade<SubClass>
    {
        SubClass Get(string name, params Expression<Func<SubClass, object>>[] includeProperties);
        IDictionary<string, int> SubClassesAsReverseDictionary(bool clearFacadeCache = false);
        void Save(SubClass[] subClasses);
    }
}