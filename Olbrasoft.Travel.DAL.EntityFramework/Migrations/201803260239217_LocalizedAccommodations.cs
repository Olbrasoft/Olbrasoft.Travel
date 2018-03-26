namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocalizedAccommodations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.LocalizedAccommodations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 70),
                        Location = c.String(maxLength: 80),
                        CheckInTime = c.String(maxLength: 10),
                        CheckOutTime = c.String(maxLength: 10),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.LanguageId })
                .ForeignKey("acco.Accommodations", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.Id)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("acco.LocalizedAccommodations", "LanguageId", "dbo.Languages");
            DropForeignKey("acco.LocalizedAccommodations", "CreatorId", "dbo.Users");
            DropForeignKey("acco.LocalizedAccommodations", "Id", "acco.Accommodations");
            DropIndex("acco.LocalizedAccommodations", new[] { "CreatorId" });
            DropIndex("acco.LocalizedAccommodations", new[] { "LanguageId" });
            DropIndex("acco.LocalizedAccommodations", new[] { "Id" });
            DropTable("acco.LocalizedAccommodations");
        }
    }
}
