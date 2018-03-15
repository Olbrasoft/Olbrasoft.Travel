namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PointsOfInterest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.PointsOfInterest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Shadow = c.Boolean(nullable: false),
                        Coordinates = c.Geography(),
                        EanId = c.Long(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.EanId, unique: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.PointsOfInterest", "CreatorId", "dbo.Users");
            DropIndex("geo.PointsOfInterest", new[] { "CreatorId" });
            DropIndex("geo.PointsOfInterest", new[] { "EanId" });
            DropTable("geo.PointsOfInterest");
        }
    }
}
