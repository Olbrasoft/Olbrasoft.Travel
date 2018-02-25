namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRegionsToRegions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.RegionsToRegions",
                c => new
                    {
                        RegionId = c.Int(nullable: false),
                        ParentRegionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RegionId, t.ParentRegionId })
                .ForeignKey("geo.Regions", t => t.ParentRegionId)
                .ForeignKey("geo.Regions", t => t.RegionId, cascadeDelete: true)
                .Index(t => t.RegionId)
                .Index(t => t.ParentRegionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.RegionsToRegions", "RegionId", "geo.Regions");
            DropForeignKey("geo.RegionsToRegions", "ParentRegionId", "geo.Regions");
            DropIndex("geo.RegionsToRegions", new[] { "ParentRegionId" });
            DropIndex("geo.RegionsToRegions", new[] { "RegionId" });
            DropTable("geo.RegionsToRegions");
        }
    }
}
