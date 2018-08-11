using System.Data.Entity.Migrations;

namespace Olbrasoft.Travel.Data.Entity.Migrations
{
    public partial class AccommodationsToAttributes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.AccommodationsToAttributes",
                c => new
                    {
                        AccommodationId = c.Int(nullable: false),
                        AttributeId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        Text = c.String(maxLength: 800),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccommodationId, t.AttributeId, t.LanguageId })
                .ForeignKey("acco.Accommodations", t => t.AccommodationId, cascadeDelete: true)
                .ForeignKey("acco.Attributes", t => t.AttributeId)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.AccommodationId)
                .Index(t => t.AttributeId)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("acco.AccommodationsToAttributes", "LanguageId", "dbo.Languages");
            DropForeignKey("acco.AccommodationsToAttributes", "CreatorId", "dbo.Users");
            DropForeignKey("acco.AccommodationsToAttributes", "AttributeId", "acco.Attributes");
            DropForeignKey("acco.AccommodationsToAttributes", "AccommodationId", "acco.Accommodations");
            DropIndex("acco.AccommodationsToAttributes", new[] { "CreatorId" });
            DropIndex("acco.AccommodationsToAttributes", new[] { "LanguageId" });
            DropIndex("acco.AccommodationsToAttributes", new[] { "AttributeId" });
            DropIndex("acco.AccommodationsToAttributes", new[] { "AccommodationId" });
            DropTable("acco.AccommodationsToAttributes");
        }
    }
}
