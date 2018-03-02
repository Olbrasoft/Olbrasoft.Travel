namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePointsOfInterestToRegions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.PointsOfInterestToRegions",
                c => new
                    {
                        PointOfInterestId = c.Int(nullable: false),
                        RegionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PointOfInterestId, t.RegionId })
                .ForeignKey("geo.PointsOfInterest", t => t.PointOfInterestId)
                .ForeignKey("geo.Regions", t => t.RegionId)
                .Index(t => t.PointOfInterestId)
                .Index(t => t.RegionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.PointsOfInterestToRegions", "RegionId", "geo.Regions");
            DropForeignKey("geo.PointsOfInterestToRegions", "PointOfInterestId", "geo.PointsOfInterest");
            DropIndex("geo.PointsOfInterestToRegions", new[] { "RegionId" });
            DropIndex("geo.PointsOfInterestToRegions", new[] { "PointOfInterestId" });
            DropTable("geo.PointsOfInterestToRegions");
        }
    }
}
