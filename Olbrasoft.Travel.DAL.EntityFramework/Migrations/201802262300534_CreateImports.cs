namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateImports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Imports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 255),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                        CurrentStatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)

                //.ForeignKey("dbo.Statuses", t => t.CurrentStatusId, cascadeDelete: true)
                .ForeignKey("dbo.Statuses", t => t.CurrentStatusId, cascadeDelete: false)
                .Index(t => t.CreatorId)
                .Index(t => t.CurrentStatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Imports", "CurrentStatusId", "dbo.Statuses");
            DropForeignKey("dbo.Imports", "CreatorId", "dbo.Users");
            DropIndex("dbo.Imports", new[] { "CurrentStatusId" });
            DropIndex("dbo.Imports", new[] { "CreatorId" });
            DropTable("dbo.Imports");
        }
    }
}
