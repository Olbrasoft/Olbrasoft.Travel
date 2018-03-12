using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class NeighborhoodsImporter : Importer<DTO.Geography.Neighborhood>
    {
        public NeighborhoodsImporter(ImportOption option) : base(option)
        {
         
        }

        public override void ImportBatch(DTO.Geography.Neighborhood[] eanNeighBorhoods)
        {
            var neighborhoodsRepository = FactoryOfRepositories.BaseRegions<Travel.DTO.Neighborhood>();
            var typeName = typeof(Neighborhood).Name;

            Logger.Log($"{typeName} Build.");
            var neighborhoods = BuildCitiesOrNeighborhoods<Neighborhood>(eanNeighBorhoods, CreatorId);
            Logger.Log(neighborhoods.Length.ToString());

            Logger.Log($"{typeName} Save.");
            neighborhoodsRepository.BulkSave(neighborhoods);
            Logger.Log($"{typeName} Saved.");

            var eanRegionIdsToIds = neighborhoodsRepository.EanRegionIdsToIds;

            typeName = typeof(LocalizedNeighborhood).Name;
            Logger.Log($"{typeName} Build.");
            var localizedNeighborhoods = BuildLocalizedCitiesOrNeighborhoods<LocalizedNeighborhood>(eanNeighBorhoods, eanRegionIdsToIds, DefaultLanguageId, CreatorId);
            Logger.Log($"{typeName} Builded:{localizedNeighborhoods.Length}.");

            Logger.Log($"{typeName} Save.");
            FactoryOfRepositories.Localized<LocalizedNeighborhood>().BulkSave(localizedNeighborhoods);
            Logger.Log($"{typeName} Saved");

        }
    }
}