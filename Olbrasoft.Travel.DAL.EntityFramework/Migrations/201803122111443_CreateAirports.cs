namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class CreateAirports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.Airports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 3),
                        Coordinates = c.Geography(nullable: false),
                        EanAirportId = c.Long(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.EanAirportId, unique: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.Airports", "CreatorId", "dbo.Users");
            DropIndex("geo.Airports", new[] { "CreatorId" });
            DropIndex("geo.Airports", new[] { "EanAirportId" });
            DropTable("geo.Airports");
        }
    }
}
