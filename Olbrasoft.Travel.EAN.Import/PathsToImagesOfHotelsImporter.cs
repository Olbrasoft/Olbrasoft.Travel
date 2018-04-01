using System.Linq;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.EAN.DTO.Property;

namespace Olbrasoft.Travel.EAN.Import
{
   class PathsToImagesOfHotelsImporter : BaseImporter<PathToHotelImage>
   {
        
       public PathsToImagesOfHotelsImporter(ImportOption option) : base(option)
       {
           
       }

        public override void Import(string path)
        {
            Logger.Log($"Load lines from {path} for import {nameof(Travel.DTO.PathToPhoto)}");

            var lines = Option.Provider.GetAllLines(path).Skip(1);
            
            Logger.Log($"Lines loaded:{lines.Count()}");


           
            
        }

   }
}