namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpatialIndexToRegionsOnCoordinates : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE SPATIAL INDEX [IX_Regions_Coordinates] ON [geo].[Regions](Coordinates)");
        }
        
        public override void Down()
        {
            Sql("DROP INDEX [IX_Regions_Coordinates] ON [geo].[Regions]");
        }
    }
}
