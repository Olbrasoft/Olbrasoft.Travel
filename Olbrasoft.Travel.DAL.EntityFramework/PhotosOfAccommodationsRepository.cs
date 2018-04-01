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

        private IReadOnlyDictionary<string, int> _pathsToIds;

        protected IReadOnlyDictionary<string, int> PathsToIds
        {
            get
            {
                return _pathsToIds ?? (_pathsToIds = AsQueryable().Select(poa => new { poa.FileName, poa.Id })
                           .ToDictionary(k => k.FileName, v => v.Id));
            }

            private set => _pathsToIds = value;
        }


        public PhotosOfAccommodationsRepository(DbContext context) : base(context)
        {
        }

       

        public void BulkSave(IEnumerable<PhotoOfAccommodation> entities, params Expression<Func<PhotoOfAccommodation, object>>[] ignorePropertiesWhenUpdating)
        {


            BulkInsert(entities);

            //entities = AddingIdsOnDependingPaths(entities.ToArray());

            //var forInsert = entities.Where(poa => poa.Id == 0).ToArray();
            //var forUpdate = entities.Where(poa => poa.Id != 0).ToArray();

            //if (forInsert.Any())
            //{
            //    BulkInsert(forInsert);
            //}

            //if (forUpdate.Any())
            //{
            //    BulkUpdate(forUpdate, ignorePropertiesWhenUpdating);
            //}
        }


        public override void ClearCache()
        {
            PathsToIds = null;
            base.ClearCache();
        }

    }
}
