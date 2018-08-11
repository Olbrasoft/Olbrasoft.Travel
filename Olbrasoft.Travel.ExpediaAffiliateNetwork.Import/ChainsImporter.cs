using System.Collections.Generic;
using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DataAccessLayer;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.Import
{
    internal class ChainsImporter : Importer
    {
        protected Queue<Chain> Chains = new Queue<Chain>();

        public ChainsImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger) 
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
        }

        protected override void RowLoaded(string[] items)
        {
            if (!int.TryParse(items[0], out var eanId) || eanId < 0) return;

            Chains.Enqueue(new Chain
            {
                EanId = eanId,
                Name = items[1],
                CreatorId = CreatorId
            });
        }

        public override void Import(string path)
        {
            LoadData(path);
            
            if (Chains.Count <= 0) return;

            LogSave<Chain>();
            FactoryOfRepositories.MappedEntities<Chain>().BulkSave(Chains);
            LogSaved<Chain>();

            Chains = null;
        }
        
    }
}