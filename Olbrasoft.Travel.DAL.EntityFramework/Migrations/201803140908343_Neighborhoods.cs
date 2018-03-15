namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Neighborhoods : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.Neighborhoods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Coordinates = c.Geography(),
                        EanId = c.Long(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.EanId, unique: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.Neighborhoods", "CreatorId", "dbo.Users");
            DropIndex("geo.Neighborhoods", new[] { "CreatorId" });
            DropIndex("geo.Neighborhoods", new[] { "EanId" });
            DropTable("geo.Neighborhoods");
        }
    }
}
