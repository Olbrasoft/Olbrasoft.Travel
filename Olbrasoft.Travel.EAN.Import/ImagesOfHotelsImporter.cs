using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DAL.EntityFramework.Migrations;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Property;

namespace Olbrasoft.Travel.EAN.Import
{

    class QuickPath
    {
        public string Path { get; set; }
    }

    class QuickExtension
    {
        public QuickExtension()
        {
            Paths = new HashSet<QuickPath>();
        }

        public string Extension { get; set; }
        public ICollection<QuickPath> Paths { get; set; }
    }


   

    internal class ImagesOfHotelsImporter : BaseImporter<HotelImage>
    {

        private IReadOnlyDictionary<int, int> _accommodationsEanIdsToIds;

        private IReadOnlyDictionary<int, int> AccommodationsEanIdsToIds =>
            _accommodationsEanIdsToIds ?? (_accommodationsEanIdsToIds =
                FactoryOfRepositories.MappedEntities<Accommodation>().EanIdsToIds);

        protected readonly HashSet<string> Paths = new HashSet<string>();
        protected readonly HashSet<string> Extensions = new HashSet<string>();
        protected readonly HashSet<string> Captions = new HashSet<string>();


        public bool IsFirstLine { get; set; } = true;


        public ImagesOfHotelsImporter(ImportOption option) : base(option)
        {

        }

        public override void Import(string path)
        {

            Provider.SplittingLine += Provider_SplittingLine;

            Logger.Log("Build data from: " + path);

            Provider.ReadToEnd(path);

            LogBuilded(Paths.Count);

        }

        private void Provider_SplittingLine(object sender, string[] items)
        {
            if (IsFirstLine)
            {
                IsFirstLine = false;
                return;
            }

            var eanHotelId = int.Parse(items[0]);

            if (!AccommodationsEanIdsToIds.ContainsKey(eanHotelId)) return;

            var caption = items[1];

            if (!string.IsNullOrEmpty(caption) && !Captions.Contains(caption)) Captions.Add(caption);

            var url = items[2];

            var path = ParsePath(url);

            if (!Paths.Contains(path)) Paths.Add(path);

            var extension = Path.GetExtension(url)?.ToLower();

            if (!Extensions.Contains(extension)) Extensions.Add(extension);
                                    
                        

        }







        void Temp()
        {

            //var pathsToPhotos = new HashSet<PathToPhoto>();

            //var result= new HashSet<PhotoOfRoom>();

            //Logger.Log("Load data from file: " + path);

            //var pathsHashSet = new HashSet<string>();
            //var captionsHashSet = new HashSet<string>();
            //var extensionsHashSet = new HashSet<string>();

            //Logger.Log("Load AccommodationsEanIdsToIds");
            //var accommodationsEanIdsToIds = FactoryOfRepositories.MappedEntities<Accommodation>().EanIdsToIds;
            //Logger.Log("Loaded");



            //using (var r = new StreamReader(path))
            //{
            //    //var parser = Option.ParserFactory.Create<HotelImage>(r.ReadLine());

            //    r.ReadLine();

            //    while (!r.EndOfStream)
            //    {
            //        var s = r.ReadLine()?.Split('|');

            //        if (s == null) continue;

            //        var eanHotelId = int.Parse(s[0]);

            //        if(!accommodationsEanIdsToIds.ContainsKey(eanHotelId)) continue;

            //        var url = s[2];

            //        var p = ParsePath(url);
            //        var c = s[1];
            //        var e = Path.GetExtension(url)?.ToLower();

            //        if (!pathsHashSet.Contains(p)) pathsHashSet.Add(p);
            //        if ( !string.IsNullOrEmpty(c) && !captionsHashSet.Contains(c)) captionsHashSet.Add(c);
            //        if (!extensionsHashSet.Contains(e)) extensionsHashSet.Add(e);

            //        //var photo = new PhotoOfRoom
            //        //{
            //        //    Id = int.Parse(s[0]),
            //        //    Path = ParsePath(url),
            //        //    File = Path.GetFileNameWithoutExtension(url),
            //        //    Extension = Path.GetExtension(url)
            //        //};


            //        //if (!result.Contains(photo))
            //        //{
            //        //    result.Add(photo);
            //        //}


            //        //if (parser.TryParse(line, out var hotelImage))
            //        //{


            //        ////    var pathToPhoto = BuildPathToPhoto(hotelImage.URL, CreatorId);

            //        ////    if (!result.Contains(pathToPhoto))
            //        ////    {
            //        ////        pathsToPhotos.Add(pathToPhoto);
            //        ////    }

            //        //    var p = ParsePath(hotelImage.URL);

            //        //    if (!result.Contains(p))
            //        //    {
            //        //        result.Add(p);
            //        //    }
            //        //}


            //        //Processing
            //    }
            //}


            //Logger.Log("Data Loaded");
            //Logger.Log($"Builded {pathsHashSet.Count} ");

            //LogBuild<PathToPhoto>();
            //var paths = pathsHashSet.Select(p => new PathToPhoto {Path = p, CreatorId = CreatorId}).ToArray();
            //var count = paths.Length;
            //LogBuilded(count);

            //LogSave<PathToPhoto>();
            //FactoryOfRepositories.PathsToPhotos().BulkSave(paths);
            //LogSaved<PathToPhoto>();

            //LogBuild<FileExtension>();
            //var extensions = extensionsHashSet.Select(p => new FileExtension {Extension = p, CreatorId = CreatorId}).ToArray();
            //count = extensions.Length;
            //LogBuilded(count);

            //LogSave<FileExtension>();
            //FactoryOfRepositories.FilesExtensions().Save(extensions);
            //LogSaved<FileExtension>();

            //LogBuild<LocalizedCaption>();
            //var localizedCaptions = captionsHashSet.Select(p => new LocalizedCaption {Text =p,DefaultLanguageId = DefaultLanguageId, CreatorId = CreatorId}).ToArray();
            //count = localizedCaptions.Length;
            //LogBuilded(count);

            //LogSave<LocalizedCaption>();
            //FactoryOfRepositories.LocalizedCaptions().BulkSave(localizedCaptions);
            //LogSaved<LocalizedCaption>();

            //Logger.Log("Load pathsToIds, extensionsToIds, captionTextsToIds");
            //var pathsToIds = FactoryOfRepositories.PathsToPhotos().PathsToIds;
            //var extensionsToIds = FactoryOfRepositories.FilesExtensions().ExtensionsToIds;
            //var captionTextsToIds = FactoryOfRepositories.LocalizedCaptions()
            //    .GetLocalizedCaptionsTextsToIds(DefaultLanguageId);
            //Logger.Log("Loaded");

            //LogBuild<PhotoOfAccommodation>();

            //var photosOfAccommodations = new Queue<PhotoOfAccommodation>();

            //using (var r = new StreamReader(path))
            //{

            //    r.ReadLine();

            //    while (!r.EndOfStream)
            //    {
            //        var s = r.ReadLine()?.Split('|');

            //        if (s == null) continue;

            //        var eanHotelId = int.Parse(s[0]);

            //        if (!accommodationsEanIdsToIds.TryGetValue(eanHotelId,out var accommodationId)) continue;

            //        var url = s[2];

            //        if (!pathsToIds.TryGetValue(ParsePath(url), out var pathToPhotoId)) continue;
            //        if (!extensionsToIds.TryGetValue(Path.GetExtension(url)?.ToLower() ?? throw new InvalidOperationException(), out var extensionId)) continue;

            //        var photoOfAccommodation = new PhotoOfAccommodation
            //        {
            //            AccommodationId = accommodationId,
            //            PathToPhotoId = pathToPhotoId,
            //            FileName = Path.GetFileNameWithoutExtension(url),
            //            FileExtensionId = extensionId,
            //            CreatorId =CreatorId
            //        };

            //        var c = s[1];

            //        if (!string.IsNullOrEmpty(c) && captionTextsToIds.TryGetValue(c, out var captionId))
            //        {
            //            photoOfAccommodation.CaptionId = captionId;
            //        }

            //        if (s[7] == "1") photoOfAccommodation.IsDefault = true;

            //        photosOfAccommodations.Enqueue(photoOfAccommodation);

            //    }
            //}

            //count = photosOfAccommodations.Count;
            //LogBuilded(count);

            //LogSave<PhotoOfAccommodation>();
            //FactoryOfRepositories.PhotosOfAccommodations().BulkSave(photosOfAccommodations);
            //LogSaved<PhotoOfAccommodation>();

        }



        public void ImportBatch(HotelImage[] eanEntities)
        {
            var eanHotelsIdsToAccommodationsIds = FactoryOfRepositories.MappedEntities<Accommodation>().EanIdsToIds;

            var eanHotelsIds = new HashSet<int>(eanHotelsIdsToAccommodationsIds.Keys);

            eanEntities = eanEntities.Where(p => eanHotelsIds.Contains(p.EANHotelID)).ToArray();

            var urls = eanEntities.Select(hi => hi.URL).ToArray();

            var pathsToIds = ImportPathsToPhotos(urls, FactoryOfRepositories.PathsToPhotos(), CreatorId);

            var extensionsToIds = ImportFilesExtensions(urls, FactoryOfRepositories.FilesExtensions(), CreatorId);

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
            return eanEntities.Where(p => !string.IsNullOrEmpty(p.Caption)).Select(p => p.Caption).Distinct().Select(p =>
                  new LocalizedCaption { Text = p, LanguageId = languageId, CreatorId = creatorId }).ToArray();
        }

        private static PhotoOfAccommodation[] BuildPhotosOfAccommodations(IEnumerable<HotelImage> eanEntities,
            IReadOnlyDictionary<int, int> eanHotelsIdsToAccommodationsIds,
            IReadOnlyDictionary<string, int> pathsToIds,
            IReadOnlyDictionary<string, int> extensionsToIds,
            IReadOnlyDictionary<string, int> captionTextsToIds,
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