namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDescriptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.Descriptions",
                c => new
                    {
                        AccommodationId = c.Int(nullable: false),
                        DescriptionTypeId = c.Int(nullable: false),
                        SupportedCultureId = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccommodationId, t.DescriptionTypeId, t.SupportedCultureId })
                .ForeignKey("acco.Accommodations", t => t.AccommodationId, cascadeDelete: true)
                .ForeignKey("acco.TypesOfDescriptions", t => t.DescriptionTypeId, cascadeDelete: true)
                .ForeignKey("dbo.SupportedCultures", t => t.SupportedCultureId, cascadeDelete: true)
                .Index(t => t.AccommodationId)
                .Index(t => t.DescriptionTypeId)
                .Index(t => t.SupportedCultureId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("acco.Descriptions", "SupportedCultureId", "dbo.SupportedCultures");
            DropForeignKey("acco.Descriptions", "TypeOfDescriptionId", "acco.TypesOfDescriptions");
            DropForeignKey("acco.Descriptions", "AccommodationId", "acco.Accommodations");
            DropIndex("acco.Descriptions", new[] { "SupportedCultureId" });
            DropIndex("acco.Descriptions", new[] { "TypeOfDescriptionId" });
            DropIndex("acco.Descriptions", new[] { "AccommodationId" });
            DropTable("acco.Descriptions");
        }
    }
}
