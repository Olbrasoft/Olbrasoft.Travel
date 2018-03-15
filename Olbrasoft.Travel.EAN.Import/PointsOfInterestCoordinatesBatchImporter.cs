using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class PointsOfInterestCoordinatesBatchImporter : BatchImporter<PointOfInterestCoordinates>
    {
        public PointsOfInterestCoordinatesBatchImporter(ImportOption option) : base(option)
        {
        }

        public override void ImportBatch(PointOfInterestCoordinates[] pointsOfInterestCoordinates)
        {
            var pointsOfInterestRepository = FactoryOfRepositories.BaseRegions<PointOfInterest>();

            ImportPointsOfInterest(pointsOfInterestCoordinates,
                pointsOfInterestRepository,
                CreatorId
                );
            
            var eanRegionIdsToIds = pointsOfInterestRepository.EanRegionIdsToIds;
            
            ImportPointsOfInterestToSubClasses(pointsOfInterestCoordinates, 
                eanRegionIdsToIds, 
                FactoryOfRepositories.BaseNames<SubClass>().NamesToIds,
                FactoryOfRepositories.ToSubClass<PointOfInterestToSubClass>(), 
                CreatorId
                );

            
            ImportLocalizedPointsOfInterest(pointsOfInterestCoordinates,
                eanRegionIdsToIds,
                FactoryOfRepositories.Localized<LocalizedPointOfInterest>(),
                DefaultLanguageId,
                CreatorId
                );

        }
        

        private void ImportLocalizedPointsOfInterest(IEnumerable<PointOfInterestCoordinates> pointsOfInterestCoordinates,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds,
            IBaseRepository<LocalizedPointOfInterest, int, int> repository,
            int languageId,
            int creatorId
            )
        {
           
           LogBuild<LocalizedPointOfInterest>();
            var localizedPointsOfInterest = BuildLocalizedRegions<LocalizedPointOfInterest>(pointsOfInterestCoordinates,
                eanRegionIdsToIds, languageId, creatorId);
            LogBuilded(localizedPointsOfInterest.Length);

            LogSave<LocalizedPointOfInterest>();
            repository.BulkSave(localizedPointsOfInterest);
            LogSaved<LocalizedPointOfInterest>();
        }



        private void ImportPointsOfInterestToSubClasses(IEnumerable<PointOfInterestCoordinates> pointsOfInterestCoordinates,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds,
            IReadOnlyDictionary<string, int> subClassesNamesToIds,
            IBaseRepository<PointOfInterestToSubClass> repository,
                int creatorId
            )
        {
            LogBuild<PointOfInterestToSubClass>();
            var pointsOfInterestToSubClasses = BuildPointsOfInterestToSubClases(pointsOfInterestCoordinates, eanRegionIdsToIds, subClassesNamesToIds, creatorId);
            LogBuilded(pointsOfInterestToSubClasses.Length);

            LogSave<PointOfInterestToSubClass>();
            repository.BulkSave(pointsOfInterestToSubClasses);
            LogSaved<PointOfInterestToSubClass>();

        }


        private void ImportPointsOfInterest(IEnumerable<PointOfInterestCoordinates> pointsOfInterestCoordinates,
            IBaseRepository<PointOfInterest> repository,
            int creatorId
            )
        {
            LogBuild<PointOfInterest>();
            var pointsOfInterest = BuildPointsOfInterest(pointsOfInterestCoordinates, creatorId);
            LogBuilded(pointsOfInterest.Length);

            LogSave<PointOfInterest>();
            repository.BulkSave(pointsOfInterest);
            LogSaved<PointOfInterest>();
        }



        private static PointOfInterestToSubClass[] BuildPointsOfInterestToSubClases(IEnumerable<PointOfInterestCoordinates> pointsOfInterestCoordinates,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds, IReadOnlyDictionary<string, int> subClassesNamesToIds, int creatorId)
        {
            var pointsOfInterestToSubClasses = new Queue<PointOfInterestToSubClass>();
            foreach (var pointOfInterestCoordinates in pointsOfInterestCoordinates.Where(p => !string.IsNullOrEmpty(p.SubClassification)))
            {
                if (!eanRegionIdsToIds.TryGetValue(pointOfInterestCoordinates.RegionID, out var id) ||
                    !subClassesNamesToIds.TryGetValue(pointOfInterestCoordinates.SubClassification.ToLower().Replace("musuems", "museums"), out var subClassId))
                {
                    throw new KeyNotFoundException();
                }

                pointsOfInterestToSubClasses.Enqueue(new PointOfInterestToSubClass
                {
                    Id = id,
                    SubClassId = subClassId,
                    CreatorId = creatorId
                });

            }
            return pointsOfInterestToSubClasses.ToArray();
        }

        private static PointOfInterest[] BuildPointsOfInterest(IEnumerable<PointOfInterestCoordinates> pointsOfInterestCoordinates, int creatorId)
        {
            return pointsOfInterestCoordinates.Select(
                p => new PointOfInterest
                {
                    EanId = p.RegionID,
                    Coordinates = CreatePoint(p.Latitude, p.Longitude),
                    CreatorId = creatorId
                }).ToArray();
        }
        
    }
}