namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpatialIndexToNeighborhoodsOnCoordinates : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE SPATIAL INDEX [IX_Neighborhoods_Coordinates] ON [geo].[Neighborhoods](Coordinates)");
        }

        public override void Down()
        {
            Sql("DROP INDEX [IX_Neighborhoods_Coordinates] ON [geo].[Neighborhoods]");
        }
    }
}
