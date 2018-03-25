namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
