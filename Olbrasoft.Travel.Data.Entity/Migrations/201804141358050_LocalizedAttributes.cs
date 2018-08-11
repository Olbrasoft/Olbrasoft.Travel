using System.Data.Entity.Migrations;

namespace Olbrasoft.Travel.Data.Entity.Migrations
{
    public partial class LocalizedAttributes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.LocalizedAttributes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 255),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.LanguageId })
                .ForeignKey("acco.Attributes", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.Id)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
        }
        
        public override void Down()
        {
            DropForeignKey("acco.LocalizedAttributes", "LanguageId", "dbo.Languages");
            DropForeignKey("acco.LocalizedAttributes", "CreatorId", "dbo.Users");
            DropForeignKey("acco.LocalizedAttributes", "Id", "acco.Attributes");
            DropIndex("acco.LocalizedAttributes", new[] { "CreatorId" });
            DropIndex("acco.LocalizedAttributes", new[] { "LanguageId" });
            DropIndex("acco.LocalizedAttributes", new[] { "Id" });
            DropTable("acco.LocalizedAttributes");
        }
    }
}
