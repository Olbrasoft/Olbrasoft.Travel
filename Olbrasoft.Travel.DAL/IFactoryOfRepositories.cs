using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IFactoryOfRepositories
    {
        ITravelRepository<T> Travel<T>() where T : class;

        IBaseRegionsRepository<T> BaseRegions<T>() where T : BaseRegion;

        IKeyIdRepository<T> KeyId<T>() where T : class, IKeyId;

        IRegionsRepository CreateRegionsRepository();
    }
}
