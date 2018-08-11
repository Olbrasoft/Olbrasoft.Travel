using System.Data.Entity.Migrations;

namespace Olbrasoft.Travel.Data.Entity.Migrations
{
    public partial class SpatialIndexToRegionsOnCenterCoordinates : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE SPATIAL INDEX [IX_Regions_CenterCoordinates] ON [geo].[Regions](CenterCoordinates)");
        }
        
        public override void Down()
        {
            Sql("DROP INDEX [IX_Regions_CenterCoordinates] ON [geo].[Regions]");
        }
    }
}
