namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class States : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.States",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        EanId = c.Long(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("geo.Countries", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.CountryId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.States", "CreatorId", "dbo.Users");
            DropForeignKey("geo.States", "CountryId", "geo.Countries");
            DropIndex("geo.States", new[] { "CreatorId" });
            DropIndex("geo.States", new[] { "CountryId" });
            DropTable("geo.States");
        }
    }
}
