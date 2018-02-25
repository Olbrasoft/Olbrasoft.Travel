namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateLocalizedAccommodations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.LocalizedAccommodations",
                c => new
                    {
                        AccommodationId = c.Int(nullable: false),
                        SupportedCultureId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 70),
                        Location = c.String(maxLength: 80),
                        CheckInTime = c.String(maxLength: 10),
                        CheckOutTime = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => new { t.AccommodationId, t.SupportedCultureId })
                .ForeignKey("acco.Accommodations", t => t.AccommodationId, cascadeDelete: true)
                .ForeignKey("dbo.SupportedCultures", t => t.SupportedCultureId, cascadeDelete: true)
                .Index(t => t.AccommodationId)
                .Index(t => t.SupportedCultureId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("acco.LocalizedAccommodations", "SupportedCultureId", "dbo.SupportedCultures");
            DropForeignKey("acco.LocalizedAccommodations", "AccommodationId", "acco.Accommodations");
            DropIndex("acco.LocalizedAccommodations", new[] { "SupportedCultureId" });
            DropIndex("acco.LocalizedAccommodations", new[] { "AccommodationId" });
            DropTable("acco.LocalizedAccommodations");
        }
    }
}
