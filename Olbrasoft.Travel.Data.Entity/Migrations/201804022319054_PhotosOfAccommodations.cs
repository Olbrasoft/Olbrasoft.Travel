namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class PhotosOfAccommodations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.PhotosOfAccommodations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccommodationId = c.Int(nullable: false),
                        PathToPhotoId = c.Int(nullable: false),
                        FileName = c.String(maxLength: 50),
                        FileExtensionId = c.Int(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                        CaptionId = c.Int(),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("acco.Accommodations", t => t.AccommodationId, cascadeDelete: true)
                .ForeignKey("dbo.Captions", t => t.CaptionId)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("dbo.FilesExtensions", t => t.FileExtensionId)
                .ForeignKey("dbo.PathsToPhotos", t => t.PathToPhotoId)
                .Index(t => t.AccommodationId)
                .Index(t => new { t.PathToPhotoId, t.FileName, t.FileExtensionId }, unique: true)
                .Index(t => t.CaptionId)
                .Index(t => t.CreatorId);
        }
        
        public override void Down()
        {
            DropForeignKey("acco.PhotosOfAccommodations", "PathToPhotoId", "dbo.PathsToPhotos");
            DropForeignKey("acco.PhotosOfAccommodations", "FileExtensionId", "dbo.FilesExtensions");
            DropForeignKey("acco.PhotosOfAccommodations", "CreatorId", "dbo.Users");
            DropForeignKey("acco.PhotosOfAccommodations", "CaptionId", "dbo.Captions");
            DropForeignKey("acco.PhotosOfAccommodations", "AccommodationId", "acco.Accommodations");
            DropIndex("acco.PhotosOfAccommodations", new[] { "CreatorId" });
            DropIndex("acco.PhotosOfAccommodations", new[] { "CaptionId" });
            DropIndex("acco.PhotosOfAccommodations", new[] { "PathToPhotoId", "FileName", "FileExtensionId" });
            DropIndex("acco.PhotosOfAccommodations", new[] { "AccommodationId" });
            DropTable("acco.PhotosOfAccommodations");
        }
    }
}
