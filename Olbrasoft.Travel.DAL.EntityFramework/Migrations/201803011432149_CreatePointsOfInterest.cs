namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePointsOfInterest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.PointsOfInterest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Shadow = c.Boolean(nullable: false),
                        SubClassId = c.Int(),
                        EanRegionId = c.Long(),
                        Coordinates = c.Geography(),
                        CreatorId = c.Int(),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("geo.SubClasses", t => t.SubClassId)
                .Index(t => t.SubClassId)
                .Index(t => t.EanRegionId, unique: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.PointsOfInterest", "SubClassId", "geo.SubClasses");
            DropForeignKey("geo.PointsOfInterest", "CreatorId", "dbo.Users");
            DropIndex("geo.PointsOfInterest", new[] { "CreatorId" });
            DropIndex("geo.PointsOfInterest", new[] { "EanRegionId" });
            DropIndex("geo.PointsOfInterest", new[] { "SubClassId" });
            DropTable("geo.PointsOfInterest");
        }
    }
}
