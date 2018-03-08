namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateContinents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.Continents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EanRegionId = c.Long(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.Continents", "CreatorId", "dbo.Users");
            DropIndex("geo.Continents", new[] { "CreatorId" });
            DropTable("geo.Continents");
        }
    }
}
