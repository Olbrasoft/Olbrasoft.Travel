using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Property;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class ImagesOfHotelsImporter : BatchImporter<HotelImage>
    {
        public ImagesOfHotelsImporter(ImportOption option) : base(option)
        {
            BatchSize = 1500000;
        }

        public override void ImportBatch(HotelImage[] eanEntities)
        {
            var eanHotelsIdsToAccommodationsIds = FactoryOfRepositories.MappedEntities<Accommodation>().EanIdsToIds;

            var eanHotelsIds = new HashSet<int>(eanHotelsIdsToAccommodationsIds.Keys);

            eanEntities = eanEntities.Where(p => eanHotelsIds.Contains(p.EANHotelID)).ToArray();
            
            var urls = eanEntities.Select(hi => hi.URL).ToArray();

            var pathsToIds = ImportPathsToPhotos(urls, FactoryOfRepositories.PathsToPhotos(), CreatorId);

            var extensionsToIds = ImportFilesExtensions(urls,FactoryOfRepositories.FilesExtensions(), CreatorId);
            
            var captionTextsToIds = ImportLocalizedCaptions(eanEntities, FactoryOfRepositories.LocalizedCaptions(),
                DefaultLanguageId, CreatorId);

            ImportPhotosOfAccommodations(eanEntities, eanHotelsIdsToAccommodationsIds, pathsToIds, extensionsToIds, captionTextsToIds);
        }
        

        private void ImportPhotosOfAccommodations(IEnumerable<HotelImage> eanEntities, IReadOnlyDictionary<int, int> eanHotelsIdsToAccommodationsIds,
            IReadOnlyDictionary<string, int> pathsToIds, IReadOnlyDictionary<string, int> extensionsToIds, IReadOnlyDictionary<string, int> captionTextsToIds)
        {
            LogBuild<PhotoOfAccommodation>();
            var photosOfAccommodations =
                BuildPhotosOfAccommodations(eanEntities, eanHotelsIdsToAccommodationsIds, pathsToIds, extensionsToIds,
                    captionTextsToIds, CreatorId);
            var count = photosOfAccommodations.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<PhotoOfAccommodation>();
            FactoryOfRepositories.PhotosOfAccommodations().BulkSave(photosOfAccommodations);
            LogSaved<PhotoOfAccommodation>();
        }


        private IReadOnlyDictionary<string, int> ImportLocalizedCaptions(IEnumerable<HotelImage> eanEntities,
            ILocalizedCaptionsRepository repository, int languageId, int creatorId)
        {

            LogBuild<LocalizedCaption>();
            var localizedCaptions = BuildLocalizedCaptions(eanEntities, DefaultLanguageId, creatorId);
            var count = localizedCaptions.Length;
            LogBuilded(count);

            if (count <= 0) return repository.GetLocalizedCaptionsTextsToIds(languageId);

            LogSaved<LocalizedCaption>();
            repository.BulkSave(localizedCaptions);
            LogSaved<LocalizedCaption>();

            return repository.GetLocalizedCaptionsTextsToIds(languageId);
        }
        

        private static LocalizedCaption[] BuildLocalizedCaptions(IEnumerable<HotelImage> eanEntities, int languageId, int creatorId)
        {
            return eanEntities.Where(p=>!string.IsNullOrEmpty(p.Caption)).Select(p => p.Caption).Distinct().Select(p =>
                new LocalizedCaption {Text = p, LanguageId = languageId, CreatorId = creatorId}).ToArray();
        }


        private static PhotoOfAccommodation[] BuildPhotosOfAccommodations(IEnumerable<HotelImage> eanEntities,
            IReadOnlyDictionary<int, int> eanHotelsIdsToAccommodationsIds,
            IReadOnlyDictionary<string, int> pathsToIds,
            IReadOnlyDictionary<string, int> extensionsToIds,
            IReadOnlyDictionary<string,int> captionTextsToIds,
                int creatorId
            )
        {
            var photosOfAccommodations = new Queue<PhotoOfAccommodation>();

            foreach (var hotelImage in eanEntities)
            {
                if (!eanHotelsIdsToAccommodationsIds.TryGetValue(hotelImage.EANHotelID, out var accommodationId)) continue;
                if (!pathsToIds.TryGetValue(ParsePath(hotelImage.URL), out var pathToPhotoId)) continue;
                if (!extensionsToIds.TryGetValue(System.IO.Path.GetExtension(hotelImage.URL)?.ToLower() ?? throw new InvalidOperationException(), out var extensionId)) continue;

                var photoOfAccommodation = new PhotoOfAccommodation
                {
                    AccommodationId = accommodationId,
                    PathToPhotoId = pathToPhotoId,
                    FileName = System.IO.Path.GetFileNameWithoutExtension(hotelImage.URL),
                    FileExtensionId = extensionId,
                    IsDefault = hotelImage.DefaultImage,
                    CreatorId = creatorId
                };

                if (!string.IsNullOrEmpty(hotelImage.Caption) && captionTextsToIds.TryGetValue(hotelImage.Caption, out var captionId))
                {
                    photoOfAccommodation.CaptionId = captionId;
                }

                photosOfAccommodations.Enqueue(photoOfAccommodation);
            }
            return photosOfAccommodations.ToArray();
        }


    }
}