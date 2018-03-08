namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateLogsOfImports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogsOfImports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Log = c.String(nullable: false, maxLength: 255),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.LogsOfImports");
        }
    }
}
