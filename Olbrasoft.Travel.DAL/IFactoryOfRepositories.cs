using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IFactoryOfRepositories
    {
        ITravelRepository<T> Travel<T>() where T : CreationInfo;

        ITypesRepository<T> BaseNames<T>() where T : CreationInfo, IHaveName;
        
        IManyToManyRepository<T> ManyToMany<T>() where T : ManyToMany;

        ILocalizedRepository<T> Localized<T>() where T : Localized;
        
        IRegionsRepository Regions();

        IRegionsToTypesRepository RegionsToTypes();

        ICountriesRepository Countries();

        IMappedEntitiesRepository<T> MappedEntities<T>() where T : CreationInfo, IHaveEanId<int>;

        IDescriptionsRepository Descriptions();

        IUsersRepository Users();

        ILanguagesRepository Languages();
    }
}
