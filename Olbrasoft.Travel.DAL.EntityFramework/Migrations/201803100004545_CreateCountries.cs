namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCountries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 2),
                        ContinentId = c.Int(nullable: false),
                        EanRegionId = c.Long(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("geo.Continents", t => t.ContinentId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.Code, unique: true)
                .Index(t => t.ContinentId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.Countries", "CreatorId", "dbo.Users");
            DropForeignKey("geo.Countries", "ContinentId", "geo.Continents");
            DropIndex("geo.Countries", new[] { "CreatorId" });
            DropIndex("geo.Countries", new[] { "ContinentId" });
            DropIndex("geo.Countries", new[] { "Code" });
            DropTable("geo.Countries");
        }
    }
}
