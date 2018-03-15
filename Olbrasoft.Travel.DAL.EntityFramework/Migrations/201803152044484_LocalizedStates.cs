namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocalizedStates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.LocalizedStates",
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
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .ForeignKey("geo.States", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.LocalizedStates", "Id", "geo.States");
            DropForeignKey("geo.LocalizedStates", "LanguageId", "dbo.Languages");
            DropForeignKey("geo.LocalizedStates", "CreatorId", "dbo.Users");
            DropIndex("geo.LocalizedStates", new[] { "CreatorId" });
            DropIndex("geo.LocalizedStates", new[] { "LanguageId" });
            DropIndex("geo.LocalizedStates", new[] { "Id" });
            DropTable("geo.LocalizedStates");
        }
    }
}
