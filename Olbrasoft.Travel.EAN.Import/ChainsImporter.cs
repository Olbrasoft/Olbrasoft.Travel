using System.Collections.Generic;
using Olbrasoft.Travel.DAL;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class ChainsImporter : Importer
    {
        protected Queue<Travel.DTO.Chain> Chains = new Queue<Travel.DTO.Chain>();

        public ChainsImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger) 
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
        }

        protected override void RowLoaded(string[] items)
        {
            if (!int.TryParse(items[0], out var eanId) || eanId < 0) return;

            Chains.Enqueue(new Travel.DTO.Chain
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

            LogSave<Travel.DTO.Chain>();
            FactoryOfRepositories.MappedEntities<Travel.DTO.Chain>().BulkSave(Chains);
            LogSaved<Travel.DTO.Chain>();

            Chains = null;
        }
        
    }
}