namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class LocalizedCaptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocalizedCaptions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Text = c.String(nullable: false, maxLength: 255),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.LanguageId })
                .ForeignKey("dbo.Captions", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.Id)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LocalizedCaptions", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.LocalizedCaptions", "CreatorId", "dbo.Users");
            DropForeignKey("dbo.LocalizedCaptions", "Id", "dbo.Captions");
            DropIndex("dbo.LocalizedCaptions", new[] { "CreatorId" });
            DropIndex("dbo.LocalizedCaptions", new[] { "LanguageId" });
            DropIndex("dbo.LocalizedCaptions", new[] { "Id" });
            DropTable("dbo.LocalizedCaptions");
        }
    }
}
