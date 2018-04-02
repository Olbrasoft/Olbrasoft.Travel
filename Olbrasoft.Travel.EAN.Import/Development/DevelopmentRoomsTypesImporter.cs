using System.Linq;
using AutoMapper;
using Olbrasoft.EntityFramework.Bulk;
using Olbrasoft.Travel.EAN.DTO.Property;

namespace Olbrasoft.Travel.EAN.Import.Development
{
    internal class DevelopmentRoomsTypesImporter : BatchImporter<RoomType>
    {
        public DevelopmentRoomsTypesImporter(ImportOption option) : base(option)
        {
            BatchSize = 320000;
            Mapper.Initialize(cfg => cfg.CreateMap<RoomType, DevelopmentRoomType>());
        }

        public override void ImportBatch(RoomType[] eanEntities)
        {
            LogBuild<DevelopmentRoomType>();
            var devRoomsTypes = eanEntities.Select(Mapper.Map<DevelopmentRoomType>).ToArray();
            var count = devRoomsTypes.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<DevelopmentRoomType>();
            using (var ctx = new DevelopmentContext())
            {
                ctx.BulkInsert(devRoomsTypes);
            }
            LogSaved<DevelopmentRoomType>();
        }
        

    }
}
