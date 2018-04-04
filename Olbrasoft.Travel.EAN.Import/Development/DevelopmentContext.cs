using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.EAN.DTO.Property;

namespace Olbrasoft.Travel.EAN.Import.Development
{
    public class DevelopmentContext : DbContext
    {
        public IDbSet<DevelopmentRoomType> DevelopmentRoomsTypes { get; set; }
        public IDbSet<DevelopmentTask> Tasks { get; set; }

        public DevelopmentContext() : base("name=Travel")
        {
            
        }


        
    }
}
