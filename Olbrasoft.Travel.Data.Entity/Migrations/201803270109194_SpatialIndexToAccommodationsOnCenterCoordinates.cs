using System.Data.Entity.Migrations;

namespace Olbrasoft.Travel.Data.Entity.Migrations
{
    public partial class SpatialIndexToAccommodationsOnCenterCoordinates : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE SPATIAL INDEX [IX_Accommodations_CenterCoordinates] ON [acco].[Accommodations](CenterCoordinates)");
        }

        public override void Down()
        {
            Sql("DROP INDEX [IX_Accommodations_CenterCoordinates] ON [acco].[Accommodations]");
        }
    }
}
