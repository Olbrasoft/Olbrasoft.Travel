using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IGeoRepository<T> : IMapToPartnersRepository<T,long> where T : Creator, IMapToPartners<long>
    {


    }
}
