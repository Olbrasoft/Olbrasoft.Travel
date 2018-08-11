using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DataAccessLayer;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.Import
{
    internal class RoomsTypesImagesImporter : BasePhotosRelationalToAccommodationImporter
    {
        

        protected HashSet<PhotoOfRoom> Photos = new HashSet<PhotoOfRoom>();
        
        public RoomsTypesImagesImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger)
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
        }

        protected override void RowLoaded(string[] items)
        {
            if (!TryBuildPhotoOfAccommodation(items, 3, out var photoOfAccommodation)) return;

           

            var photo = new PhotoOfRoom
            {
                AccommodationId = photoOfAccommodation.AccommodationId,
                PathId = photoOfAccommodation.PathToPhotoId,
                FileName = photoOfAccommodation.FileName,
                ExtensionId = photoOfAccommodation.FileExtensionId
            };
            
            Photos.Add(photo);
        }

        public override void Import(string path)
        {
            LoadData(path);

            PhotosOfAccommodations = new HashSet<PhotoOfAccommodation>(Photos.Select(p => new PhotoOfAccommodation
            {
                AccommodationId = p.AccommodationId,
                PathToPhotoId = p.PathId,
                FileName = p.FileName,
                FileExtensionId = p.ExtensionId,
                CreatorId = CreatorId

            }));
            
            var count = PhotosOfAccommodations.Count;

            WriteLog($"Builded {count} {typeof(PhotoOfAccommodation)}.");
            if (count <= 0) return;
            LogSave<PhotoOfAccommodation>();
            FactoryOfRepositories.PhotosOfAccommodations().BulkSave(PhotosOfAccommodations, p => p.CaptionId, p => p.IsDefault);
            LogSaved<PhotoOfAccommodation>();
            
        }


        public override void Dispose()
        {
            Photos = null;
            PhotosOfAccommodations = null;
            GC.SuppressFinalize(this);
            base.Dispose();
        }

    }
}