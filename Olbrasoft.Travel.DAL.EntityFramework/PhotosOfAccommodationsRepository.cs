using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class PhotosOfAccommodationsRepository : BaseRepository<PhotoOfAccommodation>, IPhotosOfAccommodationsRepository
    {
        public PhotosOfAccommodationsRepository(DbContext context) : base(context)
        {
        }



        public void BulkSave(IEnumerable<PhotoOfAccommodation> photosOfAccommodations, int batchSize,
            params Expression<Func<PhotoOfAccommodation, object>>[] ignorePropertiesWhenUpdating)
        {

            if (Exists())
            {
                var photosOfAccommodationsForStored = RebuildIds(photosOfAccommodations.ToArray());

                //var forInsert = photosOfAccommodationsForStored.Where(poa => poa.Id == 0).ToArray();
                //var forUpdate = photosOfAccommodationsForStored.Where(poa => poa.Id != 0).ToArray();

                BulkInsert(photosOfAccommodationsForStored.Where(poa => poa.Id == 0));

                BulkUpdate(photosOfAccommodationsForStored.Where(poa => poa.Id != 0), batchSize, ignorePropertiesWhenUpdating);
            }
            else
            {
                BulkInsert(photosOfAccommodations, batchSize);
            }

        }
        public void BulkSave(IEnumerable<PhotoOfAccommodation> photosOfAccommodations, params Expression<Func<PhotoOfAccommodation, object>>[] ignorePropertiesWhenUpdating)
        {
            BulkSave(photosOfAccommodations, 360000, ignorePropertiesWhenUpdating);
        }


        private PhotoOfAccommodation[] RebuildIds(PhotoOfAccommodation[] photosOfAccommodations)
        {
            var pathsAndFilesNamesToIds = GetPathIdsAndFileIdsAndExtensionIdsToIds();

            foreach (var photoOfAccommodation in photosOfAccommodations.Where(poa => poa.Id == 0))
            {
                if (pathsAndFilesNamesToIds.TryGetValue(
                    new Tuple<int, string, int>(photoOfAccommodation.PathToPhotoId, photoOfAccommodation.FileName,
                        photoOfAccommodation.FileExtensionId), out var id))
                {
                    photoOfAccommodation.Id = id;
                }
            }
            return photosOfAccommodations;
        }


        public IReadOnlyDictionary<Tuple<int, string, int>, int> GetPathIdsAndFileIdsAndExtensionIdsToIds()
        {
            return AsQueryable()
                .Select(poa => new
                {
                    poa.Id,
                    poa.PathToPhotoId,
                    poa.FileName,
                    poa.FileExtensionId
                }).ToArray()
                .ToDictionary(k => new Tuple<int, string, int>(k.PathToPhotoId, k.FileName, k.FileExtensionId),
                    v => v.Id);
        }



        //public IReadOnlyDictionary<Tuple<int, string, int>, int> GetPathIdsAndFileIdsAndExtensionIdsToIds(IEnumerable<int> pathIds)
        //{
        //    var storedPhotosOfAccommodations = new List<PhotoOfAccommodation>();

        //    foreach (var items in pathIds.SplitToEanumerableOfList(8000))
        //    {
        //        storedPhotosOfAccommodations.AddRange(
        //            AsQueryable()
        //            .Where(poa => items.Contains(poa.PathToPhotoId))
        //            .Select(poa => new
        //            {
        //                poa.Id,
        //                poa.PathToPhotoId,
        //                poa.FileName,
        //                poa.FileExtensionId
        //            }).ToArray()
        //                .Select(a => new PhotoOfAccommodation
        //                {
        //                    Id = a.Id,
        //                    PathToPhotoId = a.PathToPhotoId,
        //                    FileName = a.FileName,
        //                    FileExtensionId = a.FileExtensionId
        //                }));
        //    }

        //    return storedPhotosOfAccommodations.ToDictionary(
        //        item => new Tuple<int, string, int>(item.PathToPhotoId, item.FileName, item.FileExtensionId),
        //        item => item.Id);
        //}

    }
}
