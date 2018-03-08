namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTypesOfRegions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.TypesOfRegions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("geo.TypesOfRegions", "CreatorId", "dbo.Users");
            DropIndex("geo.TypesOfRegions", new[] { "CreatorId" });
            DropIndex("geo.TypesOfRegions", new[] { "Name" });
            DropTable("geo.TypesOfRegions");
        }
    }
}