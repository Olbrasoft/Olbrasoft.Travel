namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRegions : DbMigration
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
                        EanRegionId = c.Long(),
                        Coordinates = c.Geography(),
                        CreatorId = c.Int(),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("geo.SubClasses", t => t.SubClassId)
                .ForeignKey("geo.TypesOfRegions", t => t.TypeOfRegionId, cascadeDelete: true)
                .Index(t => t.TypeOfRegionId)
                .Index(t => t.SubClassId)
                .Index(t => t.EanRegionId, unique: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.Regions", "TypeOfRegionId", "geo.TypesOfRegions");
            DropForeignKey("geo.Regions", "SubClassId", "geo.SubClasses");
            DropForeignKey("geo.Regions", "CreatorId", "dbo.Users");
            DropIndex("geo.Regions", new[] { "CreatorId" });
            DropIndex("geo.Regions", new[] { "EanRegionId" });
            DropIndex("geo.Regions", new[] { "SubClassId" });
            DropIndex("geo.Regions", new[] { "TypeOfRegionId" });
            DropTable("geo.Regions");
        }
    }
}
