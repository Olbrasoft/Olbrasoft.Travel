namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocalizedPointsOfInterest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.LocalizedPointsOfInterest",
                c => new
                    {
                        PointOfInterestId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        LongName = c.String(maxLength: 510),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.PointOfInterestId, t.LanguageId })
                .ForeignKey("geo.PointsOfInterest", t => t.PointOfInterestId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.PointOfInterestId)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.LocalizedPointsOfInterest", "LanguageId", "dbo.Languages");
            DropForeignKey("geo.LocalizedPointsOfInterest", "CreatorId", "dbo.Users");
            DropForeignKey("geo.LocalizedPointsOfInterest", "PointOfInterestId", "geo.PointsOfInterest");
            DropIndex("geo.LocalizedPointsOfInterest", new[] { "CreatorId" });
            DropIndex("geo.LocalizedPointsOfInterest", new[] { "LanguageId" });
            DropIndex("geo.LocalizedPointsOfInterest", new[] { "PointOfInterestId" });
            DropTable("geo.LocalizedPointsOfInterest");
        }
    }
}
