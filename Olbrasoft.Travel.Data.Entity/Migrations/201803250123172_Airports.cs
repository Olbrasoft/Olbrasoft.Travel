namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Airports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.Airports",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 3),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("geo.Regions", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id, unique: true)
                .Index(t => t.Code, unique: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.Airports", "Id", "geo.Regions");
            DropForeignKey("geo.Airports", "CreatorId", "dbo.Users");
            DropIndex("geo.Airports", new[] { "CreatorId" });
            DropIndex("geo.Airports", new[] { "Code" });
            DropIndex("geo.Airports", new[] { "Id" });
            DropTable("geo.Airports");
        }
    }
}
