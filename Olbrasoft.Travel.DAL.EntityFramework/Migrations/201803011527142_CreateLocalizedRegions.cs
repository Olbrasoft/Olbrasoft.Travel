namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateLocalizedRegions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.LocalizedRegions",
                c => new
                    {
                        RegionId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        LongName = c.String(maxLength: 510),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.RegionId, t.LanguageId })
                .ForeignKey("geo.Regions", t => t.RegionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.RegionId)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.LocalizedRegions", "LanguageId", "dbo.Languages");
            DropForeignKey("geo.LocalizedRegions", "CreatorId", "dbo.Users");
            DropForeignKey("geo.LocalizedRegions", "RegionId", "geo.Regions");
            DropIndex("geo.LocalizedRegions", new[] { "CreatorId" });
            DropIndex("geo.LocalizedRegions", new[] { "LanguageId" });
            DropIndex("geo.LocalizedRegions", new[] { "RegionId" });
            DropTable("geo.LocalizedRegions");
        }
    }
}
