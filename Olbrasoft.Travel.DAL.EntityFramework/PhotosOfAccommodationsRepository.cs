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
        // private IReadOnlyDictionary<Tuple<int, string>, int> _pathsAndFilesNamesToIds;

        //protected IReadOnlyDictionary<Tuple<int, string>, int> PathsAndFileNamesToIds
        //{
        //    get
        //    {
        //        return _pathsAndFilesNamesToIds ?? (_pathsAndFilesNamesToIds = AsQueryable()
        //                   .Select(p => new { p.PathToPhotoId, p.FileName, p.Id }).ToArray()
        //                   .ToDictionary(item => new Tuple<int, string>(item.PathToPhotoId, item.FileName),
        //                       item => item.Id));
        //    }
        //    private set => _pathsAndFilesNamesToIds = value;
        //}

        //private IReadOnlyDictionary<string, int> _pathsToIds;

        //protected IReadOnlyDictionary<string, int> PathsToIds
        //{
        //    get
        //    {
        //        return _pathsToIds ?? (_pathsToIds = AsQueryable().Select(poa => new { poa.FileName, poa.Id })
        //                   .ToDictionary(k => k.FileName, v => v.Id));
        //    }
        //    private set => _pathsToIds = value;
        //}


        public PhotosOfAccommodationsRepository(DbContext context) : base(context)
        {
        }

        public void BulkSave(IEnumerable<PhotoOfAccommodation> photosOfAccommodations, params Expression<Func<PhotoOfAccommodation, object>>[] ignorePropertiesWhenUpdating)
        {
            if (!Exists())
            {
                var photosOfAccommodationsForStored = RebuildIds(photosOfAccommodations.ToArray());

                BulkInsert(photosOfAccommodationsForStored.Where(poa => poa.Id == 0));

                BulkUpdate(photosOfAccommodationsForStored.Where(poa => poa.Id != 0), ignorePropertiesWhenUpdating);
            }
            else
            {
                BulkInsert(photosOfAccommodations);
            }
        }

        
        private PhotoOfAccommodation[] RebuildIds(PhotoOfAccommodation[] photosOfAccommodations)
        {
            var pathsAndFilesNamesToIds =
                GetPathIdsAndFileIdsAndExtensionIdsToIds(photosOfAccommodations.Select(p => p.PathToPhotoId)
                    .Distinct());

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


        public IReadOnlyDictionary<Tuple<int, string, int>, int> GetPathIdsAndFileIdsAndExtensionIdsToIds(IEnumerable<int> pathIds)
        {
            var storedPhotosOfAccommodations = new List<PhotoOfAccommodation>();

            foreach (var items in pathIds.SplitToEanumerableOfList(8000))
            {
                storedPhotosOfAccommodations.AddRange(
                    AsQueryable()
                    .Where(poa => items.Contains(poa.PathToPhotoId))
                    .Select(poa => new
                    {
                        poa.Id,
                        poa.PathToPhotoId,
                        poa.FileName,
                        poa.FileExtensionId
                    }).ToArray()
                        .Select(a => new PhotoOfAccommodation
                        {
                            Id = a.Id,
                            PathToPhotoId = a.PathToPhotoId,
                            FileName = a.FileName,
                            FileExtensionId = a.FileExtensionId
                        }));
            }

            return storedPhotosOfAccommodations.ToDictionary(
                item => new Tuple<int, string, int>(item.PathToPhotoId, item.FileName, item.FileExtensionId),
                item => item.Id);
        }



        //public override void ClearCache()
        //{
        //    base.ClearCache();
        //}

    }
}
