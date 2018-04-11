namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotosOfAccommodationsToTypesOfRooms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.PhotosOfAccommodationsToTypesOfRooms",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ToId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.ToId })
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("acco.PhotosOfAccommodations", t => t.Id, cascadeDelete: true)
                .ForeignKey("acco.TypesOfRooms", t => t.ToId)
                .Index(t => t.Id)
                .Index(t => t.ToId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("acco.PhotosOfAccommodationsToTypesOfRooms", "ToId", "acco.TypesOfRooms");
            DropForeignKey("acco.PhotosOfAccommodationsToTypesOfRooms", "Id", "acco.PhotosOfAccommodations");
            DropForeignKey("acco.PhotosOfAccommodationsToTypesOfRooms", "CreatorId", "dbo.Users");
            DropIndex("acco.PhotosOfAccommodationsToTypesOfRooms", new[] { "CreatorId" });
            DropIndex("acco.PhotosOfAccommodationsToTypesOfRooms", new[] { "ToId" });
            DropIndex("acco.PhotosOfAccommodationsToTypesOfRooms", new[] { "Id" });
            DropTable("acco.PhotosOfAccommodationsToTypesOfRooms");
        }
    }
}
