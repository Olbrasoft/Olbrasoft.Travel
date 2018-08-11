using System.Data.Entity.Migrations;

namespace Olbrasoft.Travel.Data.Entity.Migrations
{
    public partial class Chains : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.Chains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EanId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 30),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.EanId, unique: true)
                .Index(t => t.CreatorId);
        }
        
        public override void Down()
        {
            DropForeignKey("acco.Chains", "CreatorId", "dbo.Users");
            DropIndex("acco.Chains", new[] { "CreatorId" });
            DropIndex("acco.Chains", new[] { "EanId" });
            DropTable("acco.Chains");
        }
    }
}
