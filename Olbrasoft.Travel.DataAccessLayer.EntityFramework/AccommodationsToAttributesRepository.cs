using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer.EntityFramework
{
    public class AccommodationsToAttributesRepository : SharpRepository.EfRepository.EfRepository<AccommodationToAttribute, int, int, int>, IAccommodationsToAttributesRepository
    {
        public event EventHandler Saved;

        public AccommodationsToAttributesRepository(DbContext dbContext) : base(dbContext)
        {
        } 

        public void BulkSave(IEnumerable<AccommodationToAttribute> accommodationsToAttributes)
        {
            var accommodationsToAttributesArray = accommodationsToAttributes as AccommodationToAttribute[] ?? accommodationsToAttributes.ToArray();

            foreach (var languageId in accommodationsToAttributesArray.GroupBy(entity => entity.LanguageId).Select(grp => grp.First())
                .Select(p => p.LanguageId))
            {

                accommodationsToAttributesArray =
                    accommodationsToAttributesArray.Where(p => p.LanguageId == languageId).ToArray();

                if (!AsQueryable().Any(l => l.LanguageId == languageId))
                {
                    Context.BulkInsert(accommodationsToAttributesArray.Where(ata => ata.LanguageId == languageId), OnSaved, 2000000);
                }
                else
                {
                    var storedAccommodationsIdsAttributesIds = FindAccommodationsIdsAttributesIds(languageId);

                    var forInsert = new List<AccommodationToAttribute>();
                    var forUpdate = new List<AccommodationToAttribute>();

                    foreach (var accommodationToAttribute in accommodationsToAttributesArray)
                    {
                        var tup = new Tuple<int, int>(accommodationToAttribute.AccommodationId, accommodationToAttribute.AttributeId);

                        if (!storedAccommodationsIdsAttributesIds.Contains(tup))
                        {
                            forInsert.Add(accommodationToAttribute);
                        }
                        else
                        {
                            forUpdate.Add(accommodationToAttribute);
                        }
                    }

                    if (forInsert.Count > 0)
                    {
                        Context.BulkInsert(forInsert, OnSaved, 2000000);
                    }

                    if (forUpdate.Count <= 0) return;
                    Context.BulkUpdate(forUpdate, OnSaved, 2000000);
                }
            }
        }


        public HashSet<Tuple<int, int>> FindAccommodationsIdsAttributesIds(int languageId)
        {
            return new HashSet<Tuple<int, int>>(AsQueryable().Where(lr => lr.LanguageId == languageId)
                .Select(ata => new { ata.AccommodationId, ata.AttributeId }).ToArray()
                .Select(p => new Tuple<int, int>(p.AccommodationId, p.AttributeId)));
        }


        protected void OnSaved(EventArgs eventArgs)
        {
            var handler = Saved;
            handler?.Invoke(this, eventArgs);
        }

    }
}