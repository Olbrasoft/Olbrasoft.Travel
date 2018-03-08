namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRegionsToregions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.RegionsToRegions",
                c => new
                    {
                        RegionId = c.Int(nullable: false),
                        ParentRegionId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.RegionId, t.ParentRegionId })
                .ForeignKey("geo.Regions", t => t.ParentRegionId)
                .ForeignKey("geo.Regions", t => t.RegionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.RegionId)
                .Index(t => t.ParentRegionId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.RegionsToRegions", "CreatorId", "dbo.Users");
            DropForeignKey("geo.RegionsToRegions", "RegionId", "geo.Regions");
            DropForeignKey("geo.RegionsToRegions", "ParentRegionId", "geo.Regions");
            DropIndex("geo.RegionsToRegions", new[] { "CreatorId" });
            DropIndex("geo.RegionsToRegions", new[] { "ParentRegionId" });
            DropIndex("geo.RegionsToRegions", new[] { "RegionId" });
            DropTable("geo.RegionsToRegions");
        }
    }
}
