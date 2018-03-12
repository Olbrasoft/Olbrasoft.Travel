namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePointsOfInterestToSubClassses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.PointsOfInterestToSubClasses",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SubClassId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("geo.PointsOfInterest", t => t.Id)
                .ForeignKey("geo.SubClasses", t => t.SubClassId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.Id)
                .Index(t => t.SubClassId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.PointsOfInterestToSubClasses", "CreatorId", "dbo.Users");
            DropForeignKey("geo.PointsOfInterestToSubClasses", "SubClassId", "geo.SubClasses");
            DropForeignKey("geo.PointsOfInterestToSubClasses", "Id", "geo.PointsOfInterest");
            DropIndex("geo.PointsOfInterestToSubClasses", new[] { "CreatorId" });
            DropIndex("geo.PointsOfInterestToSubClasses", new[] { "SubClassId" });
            DropIndex("geo.PointsOfInterestToSubClasses", new[] { "Id" });
            DropTable("geo.PointsOfInterestToSubClasses");
        }
    }
}
