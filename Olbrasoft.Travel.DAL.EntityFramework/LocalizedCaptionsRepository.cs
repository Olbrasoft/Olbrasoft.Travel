using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class LocalizedCaptionsRepository : LocalizedRepository<LocalizedCaption>, ILocalizedCaptionsRepository
    {
        private int _maxCaptionId;

        protected int MaxCaptionId
        {
            get
            {
                if (_maxCaptionId != 0) return _maxCaptionId;

                var captions = Context.Set<Caption>();
                _maxCaptionId = captions.Any() ? captions.Max(c => c.Id) : 0;

                return _maxCaptionId;
            }

            private set => _maxCaptionId = value;
        }


        public LocalizedCaptionsRepository(DbContext context) : base(context)
        {
        }

        public override void BulkSave(IEnumerable<LocalizedCaption> localizedCaptions,
            params Expression<Func<LocalizedCaption, object>>[] ignorePropertiesWhenUpdating)
        {
            var localizedCaptionsForInsert = BuildLocalizedCaptionsForInsert(localizedCaptions);

            if (!localizedCaptionsForInsert.Any()) return;

            Context.BulkInsert(localizedCaptionsForInsert.Select(lc => lc.Caption), OnSaved);
            Context.BulkInsert(localizedCaptionsForInsert, OnSaved);
        }


        private LocalizedCaption[] BuildLocalizedCaptionsForInsert(
            IEnumerable<LocalizedCaption> localizedCaptions)
        {
            var localizedCaptionsForInsert = new Queue<LocalizedCaption>();
            var localizedCaptionsArray = localizedCaptions as LocalizedCaption[] ?? localizedCaptions.ToArray();

            foreach (var languageId in localizedCaptionsArray.GroupBy(entity => entity.LanguageId).Select(grp => grp.First())
                .Select(p => p.LanguageId))
            {
                foreach (var localizedCaption in localizedCaptionsArray.Where(lc => lc.LanguageId == languageId && lc.Id == 0 && lc.Caption == null))
                {
                    if (GetLocalizedCaptionsTexts(languageId).Contains(localizedCaption.Text)) continue;

                    MaxCaptionId++;

                    localizedCaptionsForInsert.Enqueue(RebuildLocalizedCaption(localizedCaption, MaxCaptionId));
                }
            }

            return localizedCaptionsForInsert.ToArray();
        }

        private static LocalizedCaption RebuildLocalizedCaption(LocalizedCaption localizedCaption, int id)
        {
            var caption = new Caption
            {
                Id = id,
                CreatorId = localizedCaption.CreatorId
            };

            localizedCaption.Caption = caption;
            localizedCaption.Id = id;

            return localizedCaption;
        }

        protected HashSet<string> GetLocalizedCaptionsTexts(int languageId)
        {
            return new HashSet<string>(AsQueryable().Where(lc => lc.LanguageId == languageId).Select(lc => lc.Text));
        }

        public override void ClearCache()
        {
            MaxCaptionId = 0;
            base.ClearCache();
        }

        public IReadOnlyDictionary<string, int> GetLocalizedCaptionsTextsToIds(int languageId)
        {
            return AsQueryable().Where(lc => lc.LanguageId == languageId).Select(lc => new { lc.Text, lc.Id })
                .ToDictionary(k => k.Text, v => v.Id);
        }
    }
}