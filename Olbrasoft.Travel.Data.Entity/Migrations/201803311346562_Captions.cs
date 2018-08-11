using System.Data.Entity.Migrations;

namespace Olbrasoft.Travel.Data.Entity.Migrations
{
    public partial class Captions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Captions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Captions", "CreatorId", "dbo.Users");
            DropIndex("dbo.Captions", new[] { "CreatorId" });
            DropTable("dbo.Captions");
        }
    }
}
