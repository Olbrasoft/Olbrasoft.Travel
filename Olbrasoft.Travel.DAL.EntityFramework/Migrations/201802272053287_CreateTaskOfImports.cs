namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTaskOfImports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskOfImports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 255),
                        CurrentImportId = c.Int(nullable: false),
                        Progress = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Imports", t => t.CurrentImportId, cascadeDelete: true)
                .Index(t => t.CurrentImportId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskOfImports", "CurrentImportId", "dbo.Imports");
            DropIndex("dbo.TaskOfImports", new[] { "CurrentImportId" });
            DropTable("dbo.TaskOfImports");
        }
    }
}
