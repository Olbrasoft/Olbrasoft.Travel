using System.Data.Entity.Migrations;

namespace Olbrasoft.Travel.Data.Entity.Migrations
{
    public partial class LocalizedTypesOfAccommodations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.LocalizedTypesOfAccommodations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.LanguageId })
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .ForeignKey("acco.TypesOfAccommodations", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("acco.LocalizedTypesOfAccommodations", "Id", "acco.TypesOfAccommodations");
            DropForeignKey("acco.LocalizedTypesOfAccommodations", "LanguageId", "dbo.Languages");
            DropForeignKey("acco.LocalizedTypesOfAccommodations", "CreatorId", "dbo.Users");
            DropIndex("acco.LocalizedTypesOfAccommodations", new[] { "CreatorId" });
            DropIndex("acco.LocalizedTypesOfAccommodations", new[] { "LanguageId" });
            DropIndex("acco.LocalizedTypesOfAccommodations", new[] { "Id" });
            DropTable("acco.LocalizedTypesOfAccommodations");
        }
    }
}
