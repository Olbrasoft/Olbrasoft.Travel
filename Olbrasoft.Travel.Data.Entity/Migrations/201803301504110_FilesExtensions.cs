using System.Data.Entity.Migrations;

namespace Olbrasoft.Travel.Data.Entity.Migrations
{
    public partial class FilesExtensions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FilesExtensions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Extension = c.String(nullable: false, maxLength: 50),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.Extension, unique: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FilesExtensions", "CreatorId", "dbo.Users");
            DropIndex("dbo.FilesExtensions", new[] { "CreatorId" });
            DropIndex("dbo.FilesExtensions", new[] { "Extension" });
            DropTable("dbo.FilesExtensions");
        }
    }
}
