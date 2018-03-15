using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class NeighborhoodsBatchImporter : BatchImporter<DTO.Geography.NeighborhoodCoordinates>
    {
        public NeighborhoodsBatchImporter(ImportOption option) : base(option)
        {
         
        }

        public override void ImportBatch(DTO.Geography.NeighborhoodCoordinates[] eanNeighborhoodsCoordinatese)
        {
            var neighborhoodsRepository = FactoryOfRepositories.Geo<Neighborhood>();

            LogBuild<Neighborhood>();
            var neighborhoods = BuildCitiesOrNeighborhoods<Neighborhood>(eanNeighborhoodsCoordinatese, CreatorId);
            LogBuilded(neighborhoods.Length);

            LogSave<Neighborhood>();
            neighborhoodsRepository.BulkSave(neighborhoods);
            LogSaved<Neighborhood>();

            var eanIdsToIds = neighborhoodsRepository.EanIdsToIds;

            LogBuild<LocalizedNeighborhood>();
            var localizedNeighborhoods = BuildLocalizedRegions<LocalizedNeighborhood>(eanNeighborhoodsCoordinatese, eanIdsToIds, DefaultLanguageId, CreatorId);
            LogBuilded(localizedNeighborhoods.Length);

            LogSave<LocalizedNeighborhood>();
            FactoryOfRepositories.Localized<LocalizedNeighborhood>().BulkSave(localizedNeighborhoods);
            LogSaved<LocalizedNeighborhood>();

        }
    }
}