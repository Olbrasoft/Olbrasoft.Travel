using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class CitiesBatchImporter : BaseCitiesAndNeighborhoodImporter<CityCoordinates>
    {
        public CitiesBatchImporter(ImportOption option) : base(option)
        {
            TypeOfRegionId = FactoryOfRepositories.BaseNames<TypeOfRegion>().GetId("City");
            SubClassId = FactoryOfRepositories.BaseNames<SubClass>().GetId("city");
        }

        //public override void ImportBatch(CityCoordinates[] eanEntities)
        //{

        //    var repository = FactoryOfRepositories.Geo<Region>();

        //    var ids = repository.FindAll(r => r.TypeOfRegionId != 7, r => r.EanId).ToArray();


        //    var cities = new Queue<DupCity>();
        //    foreach (var eanEntity in eanEntities)
        //    {
        //        if (!ids.Contains(eanEntity.RegionID)) continue;
        //        var neighborhood = new DupCity()
        //        {
        //            RegionId = eanEntity.RegionID,
        //            RegionName = eanEntity.RegionName,
        //            Coordinates = CreatePoligon(eanEntity.Coordinates)
        //        };

        //        cities.Enqueue(neighborhood);

        //    }

        //    if (cities.Count > 0)
        //    {

        //        using (var ctx = new TravelContext())
        //        {
        //            ctx.BulkInsert(cities);
        //        }

        //    }

        //    //base.ImportBatch(eanEntities);
        //}
    }

}
