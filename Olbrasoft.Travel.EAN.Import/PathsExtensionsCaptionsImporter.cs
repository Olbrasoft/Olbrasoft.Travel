using System.Collections.Generic;
using System.IO;
using System.Linq;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
{
    public class PathsExtensionsCaptionsImporter : Importer
    {
        private IReadOnlyDictionary<int, int> _accommodationsEanIdsToIds;

        private IReadOnlyDictionary<int, int> AccommodationsEanIdsToIds =>
            _accommodationsEanIdsToIds ?? (_accommodationsEanIdsToIds =
                FactoryOfRepositories.MappedEntities<Accommodation>().EanIdsToIds);

        protected readonly HashSet<string> Paths = new HashSet<string>();
        protected readonly HashSet<string> Extensions = new HashSet<string>();
        protected readonly HashSet<string> Captions = new HashSet<string>();


        public PathsExtensionsCaptionsImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger)
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
        }

        public override void Import(string path)
        {
            LoadData(path);

            ImportPathsToPhotos(Paths, FactoryOfRepositories.PathsToPhotos(), CreatorId);

            ImportFilesExtensions(Extensions, FactoryOfRepositories.FilesExtensions(), CreatorId);

            ImportLocalizedCaptions(Captions,FactoryOfRepositories.LocalizedCaptions(),DefaultLanguageId,CreatorId);
        }


        private void ImportPathsToPhotos(IEnumerable<string> paths, IPathsToPhotosRepository repository, int creatorId)
        {
            LogBuild<PathToPhoto>();
            var pathsToPhotos = paths.Select(p => new PathToPhoto { Path = p, CreatorId = creatorId }).ToArray();
            var count = pathsToPhotos.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<PathToPhoto>();
            repository.BulkSave(pathsToPhotos);
            LogSaved<PathToPhoto>();
        }


        private void ImportFilesExtensions(IEnumerable<string> extensions, IFilesExtensionsRepository repository,
            int creatorId)
        {
            LogBuild<FileExtension>();
            var filesExtensions = extensions.Select(p => new FileExtension { Extension = p, CreatorId = creatorId }).ToArray();
            var count = filesExtensions.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<FileExtension>();
            repository.Save(filesExtensions);
            LogSaved<FileExtension>();
        }


        private void ImportLocalizedCaptions(IEnumerable<string> captions, ILocalizedCaptionsRepository repository,
            int languageId, int creatorId)
        {
            LogBuild<LocalizedCaption>();
            var localizedCaptions = captions
                .Select(p => new LocalizedCaption() { Text = p, LanguageId = languageId, CreatorId = creatorId })
                .ToArray();
            var count = localizedCaptions.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<LocalizedCaption>();
            repository.BulkSave(localizedCaptions);
            LogSaved<LocalizedCaption>();
        }


        protected override void RowLoaded(string[] items)
        {
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
    }



}
