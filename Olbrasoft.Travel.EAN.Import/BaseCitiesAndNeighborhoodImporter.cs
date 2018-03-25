using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class BaseCitiesAndNeighborhoodImporter<T> : BatchImporter<T> where T : CityNeighborhood, new()
    {
        protected int TypeOfRegionId;
        protected int SubClassId;

        protected BaseCitiesAndNeighborhoodImporter(ImportOption option) : base(option)
        {
        }

        public override void ImportBatch(T[] eanEntities)
        {
            var regionsRepository = FactoryOfRepositories.Regions();

            LogBuild<Region>();
            var regions = BuildRegions(eanEntities, CreatorId);
            var count = regions.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<Region>();
            regionsRepository.BulkSave(regions, r => r.CenterCoordinates);
            LogSaved<Region>();

            var eanIdsToIds = regionsRepository.EanIdsToIds;

            LogBuild<RegionToType>();
            var regionsToTypes = BuildRegionsToTypes(eanEntities, eanIdsToIds, TypeOfRegionId,SubClassId, CreatorId);
            count = regionsToTypes.Length;
            LogBuilded(count);

            if (count > 0)
            {
                LogSave<RegionToType>();
                FactoryOfRepositories.RegionsToTypes().BulkSave(regionsToTypes,rtt=>rtt.SubClassId);
                LogSaved<RegionToType>();
            }
            
            LogBuild<LocalizedRegion>();
            var localizedRegions = BuildLocalizedRegions(eanEntities, eanIdsToIds, DefaultLanguageId, CreatorId);
            count = localizedRegions.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<LocalizedRegion>();
            FactoryOfRepositories.Localized<LocalizedRegion>().BulkSave(localizedRegions, lr => lr.LongName);
            LogSaved<LocalizedRegion>();
        }
    }
}