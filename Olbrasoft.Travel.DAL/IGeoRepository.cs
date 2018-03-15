using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IGeoRepository<T> : IMapToPartnersRepository<T,long> where T : CreatorInfo, IMapToPartners<long>
    {


    }
}
