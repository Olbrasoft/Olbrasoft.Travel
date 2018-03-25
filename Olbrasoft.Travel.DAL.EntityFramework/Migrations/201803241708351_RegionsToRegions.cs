namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegionsToRegions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.RegionsToRegions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ToId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.ToId })
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("geo.Regions", t => t.ToId, cascadeDelete: true)
                .ForeignKey("geo.Regions", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.ToId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.RegionsToRegions", "Id", "geo.Regions");
            DropForeignKey("geo.RegionsToRegions", "ToId", "geo.Regions");
            DropForeignKey("geo.RegionsToRegions", "CreatorId", "dbo.Users");
            DropIndex("geo.RegionsToRegions", new[] { "CreatorId" });
            DropIndex("geo.RegionsToRegions", new[] { "ToId" });
            DropIndex("geo.RegionsToRegions", new[] { "Id" });
            DropTable("geo.RegionsToRegions");
        }
    }
}
