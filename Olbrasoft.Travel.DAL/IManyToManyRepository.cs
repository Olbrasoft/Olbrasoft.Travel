using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IManyToManyRepository<T> : IBaseRepository<T, int, int> where T : ManyToMany
    {
        //IReadOnlyDictionary<int, int> IdsToToIds { get; }


    }
    
}