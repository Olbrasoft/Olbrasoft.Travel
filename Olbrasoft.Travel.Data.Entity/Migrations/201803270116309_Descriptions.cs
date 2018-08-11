namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Descriptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.Descriptions",
                c => new
                    {
                        AccommodationId = c.Int(nullable: false),
                        TypeOfDescriptionId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccommodationId, t.TypeOfDescriptionId, t.LanguageId })
                .ForeignKey("acco.Accommodations", t => t.AccommodationId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .ForeignKey("acco.TypesOfDescriptions", t => t.TypeOfDescriptionId)
                .Index(t => t.AccommodationId)
                .Index(t => t.TypeOfDescriptionId)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("acco.Descriptions", "TypeOfDescriptionId", "acco.TypesOfDescriptions");
            DropForeignKey("acco.Descriptions", "LanguageId", "dbo.Languages");
            DropForeignKey("acco.Descriptions", "CreatorId", "dbo.Users");
            DropForeignKey("acco.Descriptions", "AccommodationId", "acco.Accommodations");
            DropIndex("acco.Descriptions", new[] { "CreatorId" });
            DropIndex("acco.Descriptions", new[] { "LanguageId" });
            DropIndex("acco.Descriptions", new[] { "TypeOfDescriptionId" });
            DropIndex("acco.Descriptions", new[] { "AccommodationId" });
            DropTable("acco.Descriptions");
        }
    }
}
