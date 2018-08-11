namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Accommodations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.Accommodations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SequenceNumber = c.Int(nullable: false),
                        StarRating = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Address = c.String(nullable: false, maxLength: 50),
                        AdditionalAddress = c.String(maxLength: 50),
                        CenterCoordinates = c.Geography(nullable: false),
                        TypeOfAccommodationId = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        AirportId = c.Int(),
                        ChainId = c.Int(),
                        EanId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("geo.Airports", t => t.AirportId)
                .ForeignKey("acco.Chains", t => t.ChainId)
                .ForeignKey("geo.Countries", t => t.CountryId)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .ForeignKey("acco.TypesOfAccommodations", t => t.TypeOfAccommodationId, cascadeDelete: true)
                .Index(t => t.TypeOfAccommodationId)
                .Index(t => t.CountryId)
                .Index(t => t.AirportId)
                .Index(t => t.ChainId)
                .Index(t => t.EanId, unique: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("acco.Accommodations", "TypeOfAccommodationId", "acco.TypesOfAccommodations");
            DropForeignKey("acco.Accommodations", "CreatorId", "dbo.Users");
            DropForeignKey("acco.Accommodations", "CountryId", "geo.Countries");
            DropForeignKey("acco.Accommodations", "ChainId", "acco.Chains");
            DropForeignKey("acco.Accommodations", "AirportId", "geo.Airports");
            DropIndex("acco.Accommodations", new[] { "CreatorId" });
            DropIndex("acco.Accommodations", new[] { "EanId" });
            DropIndex("acco.Accommodations", new[] { "ChainId" });
            DropIndex("acco.Accommodations", new[] { "AirportId" });
            DropIndex("acco.Accommodations", new[] { "CountryId" });
            DropIndex("acco.Accommodations", new[] { "TypeOfAccommodationId" });
            DropTable("acco.Accommodations");
        }
    }
}
