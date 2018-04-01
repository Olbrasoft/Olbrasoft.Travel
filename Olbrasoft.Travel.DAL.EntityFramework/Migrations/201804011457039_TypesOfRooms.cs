namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
            
            CreateIndex("acco.PhotosOfAccommodations", "TypeOfRoomId");
            AddForeignKey("acco.PhotosOfAccommodations", "TypeOfRoomId", "acco.TypesOfRooms", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("acco.PhotosOfAccommodations", "TypeOfRoomId", "acco.TypesOfRooms");
            DropForeignKey("acco.TypesOfRooms", "CreatorId", "dbo.Users");
            DropForeignKey("acco.TypesOfRooms", "AccommodationId", "acco.Accommodations");
            DropIndex("acco.TypesOfRooms", new[] { "CreatorId" });
            DropIndex("acco.TypesOfRooms", new[] { "AccommodationId" });
            DropIndex("acco.PhotosOfAccommodations", new[] { "TypeOfRoomId" });
            DropTable("acco.TypesOfRooms");
        }
    }
}
