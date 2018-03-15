namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocalizedContinents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.LocalizedContinents",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        LongName = c.String(maxLength: 510),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.LanguageId })
                .ForeignKey("geo.Continents", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.Id)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.LocalizedContinents", "CreatorId", "dbo.Users");
            DropForeignKey("geo.LocalizedContinents", "LanguageId", "dbo.Languages");
            DropForeignKey("geo.LocalizedContinents", "Id", "geo.Continents");
            DropIndex("geo.LocalizedContinents", new[] { "CreatorId" });
            DropIndex("geo.LocalizedContinents", new[] { "LanguageId" });
            DropIndex("geo.LocalizedContinents", new[] { "Id" });
            DropTable("geo.LocalizedContinents");
        }
    }
}
