using System.Data.Entity;
using Attribute = Olbrasoft.Travel.ExpediaAffiliateNetwork.DataTransferObject.Property.Attribute;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.Import.Development
{
    public class DevelopmentContext : DbContext
    {
        public IDbSet<DevelopmentRoomType> DevelopmentRoomsTypes { get; set; }
        public IDbSet<DevelopmentTask> Tasks { get; set; }

        public IDbSet<Attribute> Attributes { get; set; }

        public DevelopmentContext() : base("name=Travel")
        {
        }
    }
}
