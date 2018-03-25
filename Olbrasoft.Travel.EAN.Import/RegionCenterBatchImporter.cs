using System;
using System.Collections.Generic;
using Olbrasoft.Travel.DAL.EntityFramework;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class RegionCenterBatchImporter : BatchImporter<RegionCenter>
    {
        public RegionCenterBatchImporter(ImportOption option) : base(option)
        {
        }

        public override void ImportBatch(RegionCenter[] eanEntities)
        {
            var regionsRepository = FactoryOfRepositories.Regions();

            Logger.Log("Load eanIdsToIds");
            var eanIdsToIds = regionsRepository.EanIdsToIds;
            Logger.Log("Loaded");


            LogBuild<Region>();
            var regions = BuildRegions(eanEntities, eanIdsToIds);
            var count = regions.Length;
            LogBuilded(count);


            if (count <= 0) return;
            LogSave<Region>();
            regionsRepository.BulkSave(regions, r => r.Coordinates);
            LogSaved<Region>();

        }

        private static Region[] BuildRegions(IEnumerable<RegionCenter> eanEntities,
            IReadOnlyDictionary<long, int> eanIdsToIds
        )
        {
            var regions = new Queue<Region>();
            foreach (var eanEntity in eanEntities)
            {
                if (!eanIdsToIds.TryGetValue(eanEntity.RegionID, out var id)) continue;
                var region = new Region
                {
                    Id = id,
                    CenterCoordinates = CreatePoint(eanEntity.CenterLatitude, eanEntity.CenterLongitude),
                    EanId = eanEntity.RegionID
                };

                regions.Enqueue(region);
                //else
                //{
                //    using (var ctx= new TravelContext())
                //    {
                //        ctx.RegionsCenters.Add(eanEntity);
                //        ctx.SaveChanges();
                //    }

                //}


            }
            return regions.ToArray();
        }

    }
}