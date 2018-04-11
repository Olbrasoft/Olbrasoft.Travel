namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class LocalizedTypesOfRooms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.LocalizedTypesOfRooms",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.LanguageId })
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .ForeignKey("acco.TypesOfRooms", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.LanguageId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("acco.LocalizedTypesOfRooms", "Id", "acco.TypesOfRooms");
            DropForeignKey("acco.LocalizedTypesOfRooms", "LanguageId", "dbo.Languages");
            DropForeignKey("acco.LocalizedTypesOfRooms", "CreatorId", "dbo.Users");
            DropIndex("acco.LocalizedTypesOfRooms", new[] { "CreatorId" });
            DropIndex("acco.LocalizedTypesOfRooms", new[] { "LanguageId" });
            DropIndex("acco.LocalizedTypesOfRooms", new[] { "Id" });
            DropTable("acco.LocalizedTypesOfRooms");
        }
    }
}
