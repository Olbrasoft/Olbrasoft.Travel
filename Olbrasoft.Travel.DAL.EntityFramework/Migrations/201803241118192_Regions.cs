namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Regions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Coordinates = c.Geography(),
                        CenterCoordinates = c.Geography(),
                        EanId = c.Long(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.EanId, unique: true)
                .Index(t => t.CreatorId);
         
        }
        
        public override void Down()
        {
            DropForeignKey("geo.Regions", "CreatorId", "dbo.Users");
            DropIndex("geo.Regions", new[] { "CreatorId" });
            DropIndex("geo.Regions", new[] { "EanId" });
            DropTable("geo.Regions");
        }
    }
}
