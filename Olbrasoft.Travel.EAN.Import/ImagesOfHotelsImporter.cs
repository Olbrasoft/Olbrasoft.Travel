using System;
using System.Collections.Generic;
using System.Linq;
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
            
            LogBuild<FileExtension>();
            var filesExtensions = BuildFilesExtensions(eanEntities, CreatorId);
            var count = filesExtensions.Length;
            LogBuilded(count);

            var filesExtensionsRepository = FactoryOfRepositories.FilesExtensions();

            if (count > 0)
            {
                LogSave<FileExtension>();
                filesExtensionsRepository.Save(filesExtensions);
                LogSaved<FileExtension>();

            }

            var pathsToPhotosRepository = FactoryOfRepositories.PathsToPhotos();

            LogBuild<PathToPhoto>();
            var pathsPhotos = BuildPathsToPhotos(eanEntities, pathsToPhotosRepository.Paths, CreatorId);
            count = pathsPhotos.Length;
            LogBuilded(count);

            if (count > 0)
            {
                LogSave<PathToPhoto>();
                pathsToPhotosRepository.BulkSave(pathsPhotos);
                LogSaved<PathToPhoto>();
            }

            LogBuild<LocalizedCaption>();
            var localizedCaptions = BuildLocalizedCaptions(eanEntities, DefaultLanguageId, CreatorId);
            count = localizedCaptions.Length;
            LogBuilded(count);

            var localizedCaptionsRepository = FactoryOfRepositories.LocalizedCaptions();

            if (count > 0)
            {
                LogSave<LocalizedCaption>();
                localizedCaptionsRepository.BulkSave(localizedCaptions);
                LogSaved<LocalizedCaption>();
            }

            var extensionsToIds = filesExtensionsRepository.ExtensionsToIds;
            var captionTextsToIds = localizedCaptionsRepository.GetLocalizedCaptionsTextsToIds(DefaultLanguageId);

            LogBuild<PhotoOfAccommodation>();
            var photosOfAccommodations =
                BuildPhotosOfAccommodations(eanEntities, eanHotelsIdsToAccommodationsIds, pathsToPhotosRepository.PathsToIds, extensionsToIds, captionTextsToIds, CreatorId);
            count = photosOfAccommodations.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<PhotoOfAccommodation>();
            FactoryOfRepositories.PhotosOfAccommodations().BulkSave(photosOfAccommodations);
            LogSaved<PhotoOfAccommodation>();

        }
        
        private static LocalizedCaption[] BuildLocalizedCaptions(IEnumerable<HotelImage> eanEntities, int languageId, int creatorId)
        {
            return eanEntities.Where(p=>!string.IsNullOrEmpty(p.Caption)).Select(p => p.Caption).Distinct().Select(p =>
                new LocalizedCaption {Text = p, LanguageId = languageId, CreatorId = creatorId}).ToArray();
        }
        
        private static FileExtension[] BuildFilesExtensions(IEnumerable<HotelImage> eanEntities, int creatorId)
        {
            return eanEntities.Select(hi => System.IO.Path.GetExtension(hi.URL)?.ToLower()).Distinct()
                .Select(p => new FileExtension { Extension = p, CreatorId = creatorId }).ToArray();
        }


        private static PathToPhoto[] BuildPathsToPhotos(IEnumerable<HotelImage> eanEntities,
             ICollection<string> paths,
                 int creatorId
            )
        {
            var group = eanEntities.Select(p => ParsePath(p.URL)).Distinct();

            return group.Where(ptp => !paths.Contains(ptp)).Select(ptp => new PathToPhoto()
            {
                Path = ptp,
                CreatorId = creatorId

            }).ToArray();
        }
        
      
        
        PhotoOfAccommodation[] BuildPhotosOfAccommodations(IEnumerable<HotelImage> eanEntities,
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