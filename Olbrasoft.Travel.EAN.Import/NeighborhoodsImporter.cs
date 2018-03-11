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
            var typeName = typeof(Travel.DTO.Neighborhood).Name;

            Logger.Log($"{typeName} Build.");
            var neighborhoods = BuildCitiesOrNeighborhoods<Travel.DTO.Neighborhood>(eanNeighBorhoods, CreatorId);
            Logger.Log(neighborhoods.Length.ToString());

            Logger.Log($"{typeName} Save.");
            neighborhoodsRepository.BulkSave(neighborhoods);
            Logger.Log($"{typeName} Saved.");
        }
    }
}