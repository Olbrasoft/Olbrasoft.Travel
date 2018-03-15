namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocalizedCountries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.LocalizedCountries",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.LanguageId })
                .ForeignKey("geo.Countries", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.Id)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.LocalizedCountries", "CreatorId", "dbo.Users");
            DropForeignKey("geo.LocalizedCountries", "LanguageId", "dbo.Languages");
            DropForeignKey("geo.LocalizedCountries", "Id", "geo.Countries");
            DropIndex("geo.LocalizedCountries", new[] { "CreatorId" });
            DropIndex("geo.LocalizedCountries", new[] { "LanguageId" });
            DropIndex("geo.LocalizedCountries", new[] { "Id" });
            DropTable("geo.LocalizedCountries");
        }
    }
}
