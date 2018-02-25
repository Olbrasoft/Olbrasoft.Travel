namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateSupportedCultures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SupportedCultures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SupportedCultures");
        }
    }
}
