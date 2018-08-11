namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class PathsToPhotos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PathsToPhotos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(nullable: false, maxLength: 300),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.Path, unique: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PathsToPhotos", "CreatorId", "dbo.Users");
            DropIndex("dbo.PathsToPhotos", new[] { "CreatorId" });
            DropIndex("dbo.PathsToPhotos", new[] { "Path" });
            DropTable("dbo.PathsToPhotos");
        }
    }
}
