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
                        SubClassOfRegionId = c.Int(),
                        EanRegionId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("geo.SubClassesOfRegions", t => t.SubClassOfRegionId)
                .ForeignKey("geo.TypesOfRegions", t => t.TypeOfRegionId, cascadeDelete: true)
                .Index(t => t.TypeOfRegionId)
                .Index(t => t.SubClassOfRegionId)
                .Index(t => t.EanRegionId, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.Regions", "TypeOfRegionId", "geo.TypesOfRegions");
            DropForeignKey("geo.Regions", "SubClassOfRegionId", "geo.SubClassesOfRegions");
            DropIndex("geo.Regions", new[] { "EanRegionId" });
            DropIndex("geo.Regions", new[] { "SubClassOfRegionId" });
            DropIndex("geo.Regions", new[] { "TypeOfRegionId" });
            DropTable("geo.Regions");
        }
    }
}
