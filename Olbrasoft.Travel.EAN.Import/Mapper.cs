using System.Data.Entity.Spatial;
using System.Media;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO;


namespace Olbrasoft.Travel.EAN.Import
{
     public abstract class Mapper<T,TE> where T : TravelEntity 
    {
        public abstract T Map(TE eanEntity); 
        

        public static DbGeography ParsePolygon(string s)
        {
            s = s.Replace(":", ",").Replace(";", " ");
            var spl = s.Split(',');
            var firstPoint = spl[0];
            return DbGeography.PolygonFromText($"POLYGON(({s},{firstPoint}))", 4326);
        }
    }

}
