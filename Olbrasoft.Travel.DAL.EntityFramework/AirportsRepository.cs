using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
   //public class AirportsRepository : BaseRepository<Airport>, IAirportsRepository
   // {
   //     private IDictionary<long, BaseAirport> _eanAirportIdsToBaseAirports;
   //     private long _minEanAirportId = long.MinValue;

   //     private long MinEanAirportId
   //     {
   //         get
   //         {
   //             if (_minEanAirportId != long.MinValue) return _minEanAirportId;
   //             if (Exists(region => region.EanAirportId < 0))
   //             {
   //                 _minEanAirportId = Min(p => p.EanAirportId) - 1;
   //             }
   //             else
   //             {
   //                 _minEanAirportId = -1;
   //             }
   //             return _minEanAirportId;
   //         }
   //     }

   //     public AirportsRepository(TravelContext context) : base(context)
   //     {
            
   //     }

       

   //     public new void Add(Airport airport)
   //     {
   //         if (airport.EanAirportId == long.MinValue)
   //         {
   //             airport.EanAirportId = MinEanAirportId;
   //         }

   //         base.Add(airport);
   //     }

   //     private IEnumerable<Airport> Rebuild(Airport[] airports)
   //     {
   //         if (airports.All(p => p.EanAirportId != long.MinValue)) return airports;

   //         foreach (var pointOfInterest in airports.Where(p => p.EanAirportId == long.MinValue))
   //         {
   //             pointOfInterest.EanAirportId = MinEanAirportId;
   //             _minEanAirportId = _minEanAirportId - 1;
   //         }

   //         return airports;
   //     }

   //     public new void Add(IEnumerable<Airport> airports)
   //     {
   //         base.Add(Rebuild(airports.ToArray()));
   //     }

   //     public new void BulkInsert(IEnumerable<Airport> airports)
   //     {
   //         base.BulkInsert(Rebuild(airports.ToArray()));
   //     }

   //     public new void BulkUpdate(IEnumerable<Airport> airports)
   //     {
   //         base.BulkUpdate(Rebuild(airports.ToArray()));
   //     }

   //     public override void ClearCache()
   //     {
   //         _eanAirportIdsToBaseAirports = null;
   //         _minEanAirportId = long.MinValue;
   //     }

   //     public new void BulkInsertOrUpdate(Airport[] airports)
   //     {
   //       //  base.BulkInsertOrUpdate(Rebuild(airports.ToArray()).ToArray());
   //     }

   //     public IDictionary<long, BaseAirport> EanAirportsToBaseAirports()
   //     {
   //         return _eanAirportIdsToBaseAirports ?? (_eanAirportIdsToBaseAirports = AsQueryable()
   //                    .Where(poi => true).ToDictionary(poi => poi.EanAirportId, br => new BaseAirport
   //                    {
   //                        Id = br.Id,
   //                        EanAirportId = br.EanAirportId,
   //                        CreatorId = br.CreatorId,
   //                        DateAndTimeOfCreation = br.DateAndTimeOfCreation
   //                    }));
   //     }
   // }
}
