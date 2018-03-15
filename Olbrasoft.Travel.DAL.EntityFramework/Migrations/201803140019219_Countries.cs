namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Countries : DbMigration
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
                        EanId = c.Long(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("geo.Continents", t => t.ContinentId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.Code, unique: true)
                .Index(t => t.ContinentId)
                .Index(t => t.EanId, unique: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.Countries", "CreatorId", "dbo.Users");
            DropForeignKey("geo.Countries", "ContinentId", "geo.Continents");
            DropIndex("geo.Countries", new[] { "CreatorId" });
            DropIndex("geo.Countries", new[] { "EanId" });
            DropIndex("geo.Countries", new[] { "ContinentId" });
            DropIndex("geo.Countries", new[] { "Code" });
            DropTable("geo.Countries");
        }
    }
}
