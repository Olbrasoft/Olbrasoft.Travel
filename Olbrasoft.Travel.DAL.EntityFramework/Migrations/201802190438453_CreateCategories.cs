namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCategories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EanId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.EanId, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("acco.Categories", new[] { "EanId" });
            DropTable("acco.Categories");
        }
    }
}
