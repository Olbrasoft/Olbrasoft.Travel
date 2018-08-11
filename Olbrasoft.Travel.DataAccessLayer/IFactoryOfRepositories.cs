using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer
{
    public interface IFactoryOfRepositories
    {
        ITypesRepository<T> BaseNames<T>() where T : CreationInfo, IHaveName;
        
        IManyToManyRepository<T> ManyToMany<T>() where T : ManyToMany;

        ILocalizedRepository<T> Localized<T>() where T : Localized;
        
        IRegionsRepository Regions();

        IAdditionalRegionsInfoRepository<T> AdditionalRegionsInfo<T>() where T : CreatorInfo, IAdditionalRegionInfo;

        IRegionsToTypesRepository RegionsToTypes();
        
        IMappedEntitiesRepository<T> MappedEntities<T>() where T : CreationInfo, IHaveEanId<int>;

        IDescriptionsRepository Descriptions();
        
        IFilesExtensionsRepository FilesExtensions();

        IPathsToPhotosRepository PathsToPhotos();

        ILocalizedCaptionsRepository LocalizedCaptions();

        IPhotosOfAccommodationsRepository PhotosOfAccommodations();

        IAccommodationsToAttributesRepository AccommodationsToAttributes();

        IUsersRepository Users();

        ILanguagesRepository Languages();
    }
}
