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
                        Id = c.Int(nullable: false),
                        ToId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.ToId })
                .ForeignKey("geo.PointsOfInterest", t => t.Id)
                .ForeignKey("geo.Regions", t => t.ToId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.Id)
                .Index(t => t.ToId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.PointsOfInterestToRegions", "CreatorId", "dbo.Users");
            DropForeignKey("geo.PointsOfInterestToRegions", "ToId", "geo.Regions");
            DropForeignKey("geo.PointsOfInterestToRegions", "Id", "geo.PointsOfInterest");
            DropIndex("geo.PointsOfInterestToRegions", new[] { "CreatorId" });
            DropIndex("geo.PointsOfInterestToRegions", new[] { "ToId" });
            DropIndex("geo.PointsOfInterestToRegions", new[] { "Id" });
            DropTable("geo.PointsOfInterestToRegions");
        }
    }
}
