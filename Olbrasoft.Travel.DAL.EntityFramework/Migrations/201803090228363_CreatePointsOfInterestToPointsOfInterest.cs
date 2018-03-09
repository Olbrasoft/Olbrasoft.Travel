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
                        Id = c.Int(nullable: false),
                        ToId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.ToId })
                .ForeignKey("geo.PointsOfInterest", t => t.ToId, cascadeDelete: true)
                .ForeignKey("geo.PointsOfInterest", t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.Id)
                .Index(t => t.ToId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.PointsOfInterestToPointsOfInterest", "CreatorId", "dbo.Users");
            DropForeignKey("geo.PointsOfInterestToPointsOfInterest", "Id", "geo.PointsOfInterest");
            DropForeignKey("geo.PointsOfInterestToPointsOfInterest", "ToId", "geo.PointsOfInterest");
            DropIndex("geo.PointsOfInterestToPointsOfInterest", new[] { "CreatorId" });
            DropIndex("geo.PointsOfInterestToPointsOfInterest", new[] { "ToId" });
            DropIndex("geo.PointsOfInterestToPointsOfInterest", new[] { "Id" });
            DropTable("geo.PointsOfInterestToPointsOfInterest");
        }
    }
}
