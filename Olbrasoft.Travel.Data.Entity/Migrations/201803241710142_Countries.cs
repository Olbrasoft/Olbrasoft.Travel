namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Countries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 2),
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
            DropForeignKey("geo.Countries", "Id", "geo.Regions");
            DropForeignKey("geo.Countries", "CreatorId", "dbo.Users");
            DropIndex("geo.Countries", new[] { "CreatorId" });
            DropIndex("geo.Countries", new[] { "Code" });
            DropIndex("geo.Countries", new[] { "Id" });
            DropTable("geo.Countries");
        }
    }
}
