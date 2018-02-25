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
                        SupportedCultureId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        NameLong = c.String(maxLength: 510),
                    })
                .PrimaryKey(t => new { t.RegionId, t.SupportedCultureId })
                .ForeignKey("geo.Regions", t => t.RegionId, cascadeDelete: true)
                .ForeignKey("dbo.SupportedCultures", t => t.SupportedCultureId, cascadeDelete: true)
                .Index(t => t.RegionId)
                .Index(t => t.SupportedCultureId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.LocalizedRegions", "SupportedCultureId", "dbo.SupportedCultures");
            DropForeignKey("geo.LocalizedRegions", "RegionId", "geo.Regions");
            DropIndex("geo.LocalizedRegions", new[] { "SupportedCultureId" });
            DropIndex("geo.LocalizedRegions", new[] { "RegionId" });
            DropTable("geo.LocalizedRegions");
        }
    }
}
