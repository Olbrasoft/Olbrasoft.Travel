using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.EAN.DTO.Property;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class ChainsBatchImporter : BatchImporter<Chain>
    {
        public ChainsBatchImporter(ImportOption option) : base(option)
        {
        }

        public override void ImportBatch(Chain[] eanEntities)
        {

            LogBuild<Travel.DTO.Chain>();
            var chains = BuildChains(eanEntities, CreatorId);
            var count = chains.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<Travel.DTO.Chain>();
            FactoryOfRepositories.MappedEntities<Travel.DTO.Chain>().BulkSave(chains);
            LogSaved<Travel.DTO.Chain>();

        }


        private static Travel.DTO.Chain[] BuildChains(IEnumerable<Chain> eanChains,
            int creatorId
            )
        {
            return eanChains.Where(ch=>ch.ChainCodeID>=0).Select(ch => new Travel.DTO.Chain()
            {
                EanId = ch.ChainCodeID,
                Name = ch.ChainName,
                CreatorId = creatorId
            }).ToArray();

        }

    }
}
