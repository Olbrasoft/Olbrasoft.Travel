namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateLanguages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        EanLanguageCode = c.String(maxLength: 5),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.EanLanguageCode, unique: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Languages", "CreatorId", "dbo.Users");
            DropIndex("dbo.Languages", new[] { "CreatorId" });
            DropIndex("dbo.Languages", new[] { "EanLanguageCode" });
            DropTable("dbo.Languages");
        }
    }
}
