namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpatialIndexToCitiesOnCoordinates : DbMigration
    {
        public override void Up()
        {
            // Sql("CREATE SPATIAL INDEX [IX_Cities_Coordinates] ON [geo].[Cities](Coordinates)");
        }

        public override void Down()
        {
            //  Sql("DROP INDEX [IX_Cities_Coordinates] ON [geo].[Cities]");
        }
    }
}
