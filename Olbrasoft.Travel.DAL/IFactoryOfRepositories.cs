using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IFactoryOfRepositories
    {
        IBaseRegionsRepository<T> BaseRegions<T>() where T : BaseRegion;

        IBaseNamesRepository<T> BaseNames<T>() where T : BaseName;

        IManyToManyRepository<T> ManyToMany<T>() where T : ManyToMany;

        ILocalizedRepository<T> Localized<T>() where T : BaseLocalized;

        IRegionsRepository CreateRegionsRepository();
    }
}
