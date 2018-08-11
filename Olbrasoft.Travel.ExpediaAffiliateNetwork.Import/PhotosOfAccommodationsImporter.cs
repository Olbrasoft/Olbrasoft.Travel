using System;
using System.Collections.Generic;
using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DataAccessLayer;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.Import
{
    internal class PhotosOfAccommodationsImporter : BasePhotosRelationalToAccommodationImporter
    {
        private IReadOnlyDictionary<string, int> _captionsToIds;

        protected IReadOnlyDictionary<string, int> CaptionsToIds
        {
            get =>
                _captionsToIds ?? (_captionsToIds = FactoryOfRepositories.LocalizedCaptions()
                    .GetLocalizedCaptionsTextsToIds(DefaultLanguageId));

            set => _captionsToIds = value;
        }
        
        public PhotosOfAccommodationsImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories,
            SharedProperties sharedProperties, ILoggingImports logger)
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {

        }

        protected override void RowLoaded(string[] items)
        {
            if (!TryBuildPhotoOfAccommodation(items, 2, out var photoOfAccommodation)) return;

            photoOfAccommodation = IfExistsCaptionAddLink(photoOfAccommodation, CaptionsToIds, items[1]);

            PhotosOfAccommodations.Add(photoOfAccommodation);
        }

        private static PhotoOfAccommodation IfExistsCaptionAddLink(PhotoOfAccommodation photoOfAccommodation,
            IReadOnlyDictionary<string, int> captionsToIds, string caption)
        {
            if (!string.IsNullOrEmpty(caption) && captionsToIds.TryGetValue(caption, out var captionId))
            {
                photoOfAccommodation.CaptionId = captionId;
            }

            return photoOfAccommodation;
        }


        public override void Import(string path)
        {
            LoadData(path);
            var count = PhotosOfAccommodations.Count;

            WriteLog($"Builded {count} {typeof(PhotoOfAccommodation)}.");

            if (count <= 0) return;
            LogSave<PhotoOfAccommodation>();
            FactoryOfRepositories.PhotosOfAccommodations().BulkSave(PhotosOfAccommodations);
            LogSaved<PhotoOfAccommodation>();
        }


        public override void Dispose()
        {
            CaptionsToIds = null;

            GC.SuppressFinalize(this);
            base.Dispose();
        }
    }
}
