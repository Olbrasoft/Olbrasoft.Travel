using System.Data.Entity.Migrations;

namespace Olbrasoft.Travel.Data.Entity.Migrations
{
    public partial class LocalizedRegions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.LocalizedRegions",
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
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .ForeignKey("geo.Regions", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.LocalizedRegions", "Id", "geo.Regions");
            DropForeignKey("geo.LocalizedRegions", "LanguageId", "dbo.Languages");
            DropForeignKey("geo.LocalizedRegions", "CreatorId", "dbo.Users");
            DropIndex("geo.LocalizedRegions", new[] { "CreatorId" });
            DropIndex("geo.LocalizedRegions", new[] { "LanguageId" });
            DropIndex("geo.LocalizedRegions", new[] { "Id" });
            DropTable("geo.LocalizedRegions");
        }
    }
}
