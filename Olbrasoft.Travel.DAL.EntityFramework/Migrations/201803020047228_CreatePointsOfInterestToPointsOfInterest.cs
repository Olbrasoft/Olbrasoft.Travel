namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePointsOfInterestToPointsOfInterest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.PointsOfInterestToPointsOfInterestRepository",
                c => new
                    {
                        PointOfInterestId = c.Int(nullable: false),
                        ParentPointOfInterestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PointOfInterestId, t.ParentPointOfInterestId })
                .ForeignKey("geo.PointsOfInterest", t => t.ParentPointOfInterestId)
                .ForeignKey("geo.PointsOfInterest", t => t.PointOfInterestId, cascadeDelete: true)
                .Index(t => t.PointOfInterestId)
                .Index(t => t.ParentPointOfInterestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.PointsOfInterestToPointsOfInterestRepository", "PointOfInterestId", "geo.PointsOfInterest");
            DropForeignKey("geo.PointsOfInterestToPointsOfInterestRepository", "ParentPointOfInterestId", "geo.PointsOfInterest");
            DropIndex("geo.PointsOfInterestToPointsOfInterestRepository", new[] { "ParentPointOfInterestId" });
            DropIndex("geo.PointsOfInterestToPointsOfInterestRepository", new[] { "PointOfInterestId" });
            DropTable("geo.PointsOfInterestToPointsOfInterestRepository");
        }
    }
}
