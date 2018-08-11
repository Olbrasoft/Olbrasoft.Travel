using System.Data.Entity.Migrations;

namespace Olbrasoft.Travel.Data.Entity.Migrations
{
    public partial class TypesOfAccommodations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.TypesOfAccommodations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EanId = c.Int(nullable: false),
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
            DropForeignKey("acco.TypesOfAccommodations", "CreatorId", "dbo.Users");
            DropIndex("acco.TypesOfAccommodations", new[] { "CreatorId" });
            DropIndex("acco.TypesOfAccommodations", new[] { "EanId" });
            DropTable("acco.TypesOfAccommodations");
        }
    }
}
