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
                        Id = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        LongName = c.String(maxLength: 510),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.LanguageId })
                .ForeignKey("geo.PointsOfInterest", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.Id)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.LocalizedPointsOfInterest", "CreatorId", "dbo.Users");
            DropForeignKey("geo.LocalizedPointsOfInterest", "LanguageId", "dbo.Languages");
            DropForeignKey("geo.LocalizedPointsOfInterest", "Id", "geo.PointsOfInterest");
            DropIndex("geo.LocalizedPointsOfInterest", new[] { "CreatorId" });
            DropIndex("geo.LocalizedPointsOfInterest", new[] { "LanguageId" });
            DropIndex("geo.LocalizedPointsOfInterest", new[] { "Id" });
            DropTable("geo.LocalizedPointsOfInterest");
        }
    }
}
