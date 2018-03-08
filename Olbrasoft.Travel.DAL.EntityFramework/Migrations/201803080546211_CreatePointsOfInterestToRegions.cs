namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
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
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.PointOfInterestId, t.RegionId })
                .ForeignKey("geo.PointsOfInterest", t => t.PointOfInterestId)
                .ForeignKey("geo.Regions", t => t.RegionId)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.PointOfInterestId)
                .Index(t => t.RegionId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.PointsOfInterestToRegions", "CreatorId", "dbo.Users");
            DropForeignKey("geo.PointsOfInterestToRegions", "RegionId", "geo.Regions");
            DropForeignKey("geo.PointsOfInterestToRegions", "PointOfInterestId", "geo.PointsOfInterest");
            DropIndex("geo.PointsOfInterestToRegions", new[] { "CreatorId" });
            DropIndex("geo.PointsOfInterestToRegions", new[] { "RegionId" });
            DropIndex("geo.PointsOfInterestToRegions", new[] { "PointOfInterestId" });
            DropTable("geo.PointsOfInterestToRegions");
        }
    }
}
