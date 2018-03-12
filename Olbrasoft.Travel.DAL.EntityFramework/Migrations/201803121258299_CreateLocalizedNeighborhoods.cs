namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateLocalizedNeighborhoods : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.LocalizedNeighborhoods",
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
                .ForeignKey("geo.Neighborhoods", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.Id)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.LocalizedNeighborhoods", "CreatorId", "dbo.Users");
            DropForeignKey("geo.LocalizedNeighborhoods", "LanguageId", "dbo.Languages");
            DropForeignKey("geo.LocalizedNeighborhoods", "Id", "geo.Neighborhoods");
            DropIndex("geo.LocalizedNeighborhoods", new[] { "CreatorId" });
            DropIndex("geo.LocalizedNeighborhoods", new[] { "LanguageId" });
            DropIndex("geo.LocalizedNeighborhoods", new[] { "Id" });
            DropTable("geo.LocalizedNeighborhoods");
        }
    }
}
