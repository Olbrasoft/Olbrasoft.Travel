namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Coordinates = c.Geography(),
                        EanRegionId = c.Long(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.EanRegionId, unique: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.Cities", "CreatorId", "dbo.Users");
            DropIndex("geo.Cities", new[] { "CreatorId" });
            DropIndex("geo.Cities", new[] { "EanRegionId" });
            DropTable("geo.Cities");
        }
    }
}
