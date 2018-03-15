namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpatialIndexToPointsOfInterestOnCoordinates : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE SPATIAL INDEX [IX_PointsOfInterest_Coordinates] ON [geo].[PointsOfInterest](Coordinates)");
        }

        public override void Down()
        {
            Sql("DROP INDEX [IX_PointsOfInterest_Coordinates] ON [geo].[PointsOfInterest]");
        }
    }
}
