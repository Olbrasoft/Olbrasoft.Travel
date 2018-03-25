namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegionsToTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.RegionsToTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ToId = c.Int(nullable: false),
                        SubClassId = c.Int(),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.ToId })
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("geo.Regions", t => t.Id, cascadeDelete: true)
                .ForeignKey("geo.SubClasses", t => t.SubClassId)
                .ForeignKey("geo.TypesOfRegions", t => t.ToId)
                .Index(t => t.Id)
                .Index(t => t.ToId)
                .Index(t => t.SubClassId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.RegionsToTypes", "ToId", "geo.TypesOfRegions");
            DropForeignKey("geo.RegionsToTypes", "SubClassId", "geo.SubClasses");
            DropForeignKey("geo.RegionsToTypes", "Id", "geo.Regions");
            DropForeignKey("geo.RegionsToTypes", "CreatorId", "dbo.Users");
            DropIndex("geo.RegionsToTypes", new[] { "CreatorId" });
            DropIndex("geo.RegionsToTypes", new[] { "SubClassId" });
            DropIndex("geo.RegionsToTypes", new[] { "ToId" });
            DropIndex("geo.RegionsToTypes", new[] { "Id" });
            DropTable("geo.RegionsToTypes");
        }
    }
}
