namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePointsOfInterestToPointsOfInterest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.PointsOfInterestToPointsOfInterest",
                c => new
                    {
                        PointOfInterestId = c.Int(nullable: false),
                        ParentPointOfInterestId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.PointOfInterestId, t.ParentPointOfInterestId })
                .ForeignKey("geo.PointsOfInterest", t => t.ParentPointOfInterestId)
                .ForeignKey("geo.PointsOfInterest", t => t.PointOfInterestId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.PointOfInterestId)
                .Index(t => t.ParentPointOfInterestId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.PointsOfInterestToPointsOfInterest", "CreatorId", "dbo.Users");
            DropForeignKey("geo.PointsOfInterestToPointsOfInterest", "PointOfInterestId", "geo.PointsOfInterest");
            DropForeignKey("geo.PointsOfInterestToPointsOfInterest", "ParentPointOfInterestId", "geo.PointsOfInterest");
            DropIndex("geo.PointsOfInterestToPointsOfInterest", new[] { "CreatorId" });
            DropIndex("geo.PointsOfInterestToPointsOfInterest", new[] { "ParentPointOfInterestId" });
            DropIndex("geo.PointsOfInterestToPointsOfInterest", new[] { "PointOfInterestId" });
            DropTable("geo.PointsOfInterestToPointsOfInterest");
        }
    }
}
