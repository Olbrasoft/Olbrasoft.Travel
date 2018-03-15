namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Regions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeOfRegionId = c.Int(nullable: false),
                        SubClassId = c.Int(),
                        EanId = c.Long(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("geo.SubClasses", t => t.SubClassId)
                .ForeignKey("geo.TypesOfRegions", t => t.TypeOfRegionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.TypeOfRegionId)
                .Index(t => t.SubClassId)
                .Index(t => t.EanId, unique: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.Regions", "CreatorId", "dbo.Users");
            DropForeignKey("geo.Regions", "TypeOfRegionId", "geo.TypesOfRegions");
            DropForeignKey("geo.Regions", "SubClassId", "geo.SubClasses");
            DropIndex("geo.Regions", new[] { "CreatorId" });
            DropIndex("geo.Regions", new[] { "EanId" });
            DropIndex("geo.Regions", new[] { "SubClassId" });
            DropIndex("geo.Regions", new[] { "TypeOfRegionId" });
            DropTable("geo.Regions");
        }
    }
}
