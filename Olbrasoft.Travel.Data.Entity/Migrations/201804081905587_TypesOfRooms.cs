using System.Data.Entity.Migrations;

namespace Olbrasoft.Travel.Data.Entity.Migrations
{
    public partial class TypesOfRooms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.TypesOfRooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccommodationId = c.Int(nullable: false),
                        EanId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("acco.Accommodations", t => t.AccommodationId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.AccommodationId)
                .Index(t => t.CreatorId);
        }
        
        public override void Down()
        {
            DropForeignKey("acco.TypesOfRooms", "CreatorId", "dbo.Users");
            DropForeignKey("acco.TypesOfRooms", "AccommodationId", "acco.Accommodations");
            DropIndex("acco.TypesOfRooms", new[] { "CreatorId" });
            DropIndex("acco.TypesOfRooms", new[] { "AccommodationId" });
            DropTable("acco.TypesOfRooms");
        }
    }
}
