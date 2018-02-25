namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class AddColumnCoordinatesToRegions : DbMigration
    {
        public override void Up()
        {
            AddColumn("geo.Regions", "Coordinates", c => c.Geography());
            //added manual Jiøí Tùma 24.2.2018
            Sql("CREATE SPATIAL INDEX [IX_Regions_Coordinates] ON [geo].[Regions](Coordinates)");
        }
        
        public override void Down()
        {
            //added manual Jiøí Tùma 24.2.2018
            Sql("DROP INDEX [IX_Regions_Coordinates] ON [geo].[Regions]");
            DropColumn("geo.Regions", "Coordinates");
        }
    }
}
