using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IFactoryOfRepositories
    {
        IBaseRegionsRepository<T> BaseRegions<T>() where T : BaseGeo;

        IBaseNamesRepository<T> BaseNames<T>() where T : BaseName;

        IManyToManyRepository<T> ManyToMany<T>() where T : ManyToMany;

        IToSubClassesRepository<T> ToSubClass<T>() where T : ToSubClass;

        ILocalizedRepository<T> Localized<T>() where T : BaseLocalized;

        IGeoRepository<T> Geo<T>() where T : Creator, IMapToPartners<long>;
            
        IUsersRepository Users();

        IRegionsRepository CreateRegionsRepository();
    }
}
