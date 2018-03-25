using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface ICountriesRepository : ITravelRepository<Country>
    {
        IReadOnlyDictionary<string, int> CodesToIds { get; }
    }
}