using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.BLL;
using Olbrasoft.Travel.DTO;
using PointOfInterest = Olbrasoft.Travel.EAN.DTO.Geography.PointOfInterest;

namespace Olbrasoft.Travel.EAN.Import
{
    class PointsOfInterestImporter : Importer<PointOfInterest>
    {
        protected new readonly ParentRegionImportOption Option;
        protected readonly ISubClassesFacade SubClassesFacade;
        protected readonly IPointsOfInterestFacade PointsOfInterestFacade;


        public PointsOfInterestImporter(ParentRegionImportOption option) : base(option)
        {
            Option = option;
            SubClassesFacade = Option.SubClassesFacade;
            PointsOfInterestFacade = Option.PointsOfInterestFacade;
        }

        protected override void ImportBatch(PointOfInterest[] parentRegions)
        {
            const string subClassName = "neighbor";

            if (!SubClassesFacade.SubClassesAsReverseDictionary().TryGetValue(subClassName, out var subClassId))
            {
                WriteLog($"Id for {subClassName} is 0 import will be terminated.");
                return;
            }

            var defaultBasePointOfInterest = new BasePointOfInterest
            {
                SubClassId = subClassId,
                Shadow = false,
                CreatorId = CreatorId
            };

            var adeptsToLocalizedPointsOfInterest =
                ImportPointsOfInterest(parentRegions, PointsOfInterestFacade, defaultBasePointOfInterest,
                    SubClassesFacade.SubClassesAsReverseDictionary(true));

            ImportLocalizedRegions(adeptsToLocalizedPointsOfInterest, LocalizedFacade,
                PointsOfInterestFacade.GetMappingEanRegionIdsToIds(true), DefaultLanguageId, CreatorId);

        }

        void ImportLocalizedRegions(
            IDictionary<long, LocalizedPointOfInterest> adeptsToLocalizedPointsOfInterest,
            ILocalizedFacade localizedFacade,
            IDictionary<long, int> eanRegionIdsToPointsOfInterestIds,
            int defaultLanguageId,
            int creatorId
        )
        {
            WriteLog($"LocalizedPoinstsOfInterest Build.");
            foreach (var adeptToLocalizedRegion in adeptsToLocalizedPointsOfInterest)
            {
                if (!eanRegionIdsToPointsOfInterestIds.TryGetValue(adeptToLocalizedRegion.Key,
                    out var pointOfInterestId)) continue;
                adeptToLocalizedRegion.Value.Id = pointOfInterestId;
                adeptToLocalizedRegion.Value.LanguageId = defaultLanguageId;
                adeptToLocalizedRegion.Value.CreatorId = creatorId;
                adeptToLocalizedRegion.Value.DateAndTimeOfCreation = DateTime.Now;
            }

            var localizedPointsOfInterest = adeptsToLocalizedPointsOfInterest.Values.Where(p => p.Id != 0).ToArray();
            var count = localizedPointsOfInterest.Length;
            WriteLog($"LocalizedPoinstsOfInterest Builded:{count} items to insert.");

            if (count <= 0) return;
            WriteLog("LocalizedPoinstsOfInterest Save.");
            localizedFacade.BulkSave(localizedPointsOfInterest);
            WriteLog("LocalizedPoinstsOfInterest Save.");
        }




        private IDictionary<long, LocalizedPointOfInterest> ImportPointsOfInterest(
            IEnumerable<PointOfInterest> eanPointsOfInterest,
            IPointsOfInterestFacade pointsOfInterestFacade,
            BasePointOfInterest defaultBasePointOfInterest,
            IDictionary<string, int> storedSubClasses)
        {
            WriteLog($"PoinstsOfInterest Build.");
            var pointsOfInterest = BuilPointsOfInterest(
                 eanPointsOfInterest
                 , pointsOfInterestFacade.EanRegionIdsToBasePointsOfInterest()
                 , defaultBasePointOfInterest,
                 storedSubClasses, out var adeptsToLocalizedPointsOfInterests);

            var count = pointsOfInterest.Length;
            var countNew = pointsOfInterest.Count(poi => poi.Id == 0);

            WriteLog($"PoinstsOfInterest Builded: {countNew} items to insert and {count-countNew} items to update.");

            if (count <= 0) return adeptsToLocalizedPointsOfInterests;
            WriteLog("PoinstsOfInterest Save.");
            pointsOfInterestFacade.BulkSave(pointsOfInterest);
            WriteLog("PoinstsOfInterest Saved.");

            return adeptsToLocalizedPointsOfInterests;
        }


        private static Travel.DTO.PointOfInterest[] BuilPointsOfInterest(
            IEnumerable<PointOfInterest> eanPointsOfInterest,
            IDictionary<long, BasePointOfInterest> storedPoi,
            BasePointOfInterest defaultBasePointOfInterest,
            IDictionary<string, int> storedSubClasses,
            out IDictionary<long, LocalizedPointOfInterest> adeptsToLocalizedPointsOfInterest)
        {
            var pointsOfInterest = new List<Travel.DTO.PointOfInterest>();
            adeptsToLocalizedPointsOfInterest = new Dictionary<long, LocalizedPointOfInterest>();

            foreach (var eanPoi in eanPointsOfInterest)
            {
                var poi = new Travel.DTO.PointOfInterest { EanRegionId = eanPoi.RegionID };
                if (storedPoi.TryGetValue(eanPoi.RegionID, out var basePoi))
                {
                    poi.Id = basePoi.Id;
                    poi.CreatorId = basePoi.CreatorId;
                    poi.DateAndTimeOfCreation = basePoi.DateAndTimeOfCreation;

                    if (basePoi.SubClassId != 0) poi.SubClassId = basePoi.SubClassId;
                }
                else
                {
                    poi.CreatorId = defaultBasePointOfInterest.CreatorId;
                    poi.DateAndTimeOfCreation = DateTime.Now;

                    if (!string.IsNullOrEmpty(eanPoi.SubClassification))
                    {
                        poi.SubClassId = storedSubClasses
                       .TryGetValue(eanPoi.SubClassification.ToLower()
                       .Replace("musuems", "museums"), out var subClassId) ? subClassId : defaultBasePointOfInterest.SubClassId;
                    }
                    else
                    {
                        poi.SubClassId = defaultBasePointOfInterest.SubClassId;
                    }

                    if (adeptsToLocalizedPointsOfInterest.ContainsKey(eanPoi.RegionID)) continue;
                    var localizedPoi = new LocalizedPointOfInterest
                    {
                        Name = eanPoi.RegionName,
                        LongName = eanPoi.RegionNameLong
                    };
                    adeptsToLocalizedPointsOfInterest.Add(eanPoi.RegionID, localizedPoi);
                }

                poi.Coordinates = CreatePoint(eanPoi.Latitude, eanPoi.Longitude);
                pointsOfInterest.Add(poi);
            }

            return pointsOfInterest.ToArray();
        }
    }
}