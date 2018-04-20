using System.Collections.Generic;
using Olbrasoft.Travel.DAL;

namespace Olbrasoft.Travel.EAN.Import
{
    internal abstract class BaseNamesImporter : Importer
    {
        protected abstract int NameIndex { get; }

        protected IDictionary<int, string> EanIdsToNames = new Dictionary<int, string>();
        
        protected BaseNamesImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger) 
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {

        }

        protected override void RowLoaded(string[] items)
        {
            if (!int.TryParse(items[0], out var eanId)) return;

            EanIdsToNames.Add(eanId, items[NameIndex]);
        }

    }
}