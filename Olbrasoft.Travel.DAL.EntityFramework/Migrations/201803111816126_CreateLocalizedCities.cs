namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateLocalizedCities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.LocalizedCities",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        LongName = c.String(maxLength: 510),
                        Name = c.String(nullable: false, maxLength: 255),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.LanguageId })
                .ForeignKey("geo.Cities", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.Id)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.LocalizedCities", "CreatorId", "dbo.Users");
            DropForeignKey("geo.LocalizedCities", "LanguageId", "dbo.Languages");
            DropForeignKey("geo.LocalizedCities", "Id", "geo.Cities");
            DropIndex("geo.LocalizedCities", new[] { "CreatorId" });
            DropIndex("geo.LocalizedCities", new[] { "LanguageId" });
            DropIndex("geo.LocalizedCities", new[] { "Id" });
            DropTable("geo.LocalizedCities");
        }
    }
}
