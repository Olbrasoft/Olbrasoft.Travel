namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateLocalizedContinents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.LocalizedContinents",
                c => new
                    {
                        ContinentId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        LongName = c.String(maxLength: 510),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.ContinentId, t.LanguageId })
                .ForeignKey("geo.Continents", t => t.ContinentId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.ContinentId)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.LocalizedContinents", "CreatorId", "dbo.Users");
            DropForeignKey("geo.LocalizedContinents", "LanguageId", "dbo.Languages");
            DropForeignKey("geo.LocalizedContinents", "ContinentId", "geo.Continents");
            DropIndex("geo.LocalizedContinents", new[] { "CreatorId" });
            DropIndex("geo.LocalizedContinents", new[] { "LanguageId" });
            DropIndex("geo.LocalizedContinents", new[] { "ContinentId" });
            DropTable("geo.LocalizedContinents");
        }
    }
}
