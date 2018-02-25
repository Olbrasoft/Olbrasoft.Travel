namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocalizedCategories : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.SupportedCultures");
            CreateTable(
                "acco.LocalizedCategories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false),
                        SupportedCultureId = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => new { t.CategoryId, t.SupportedCultureId })
                .ForeignKey("acco.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.SupportedCultures", t => t.SupportedCultureId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.SupportedCultureId);
            
            AlterColumn("dbo.SupportedCultures", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.SupportedCultures", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("acco.LocalizedCategories", "SupportedCultureId", "dbo.SupportedCultures");
            DropForeignKey("acco.LocalizedCategories", "CategoryId", "acco.Categories");
            DropIndex("acco.LocalizedCategories", new[] { "SupportedCultureId" });
            DropIndex("acco.LocalizedCategories", new[] { "CategoryId" });
            DropPrimaryKey("dbo.SupportedCultures");
            AlterColumn("dbo.SupportedCultures", "Id", c => c.Int(nullable: false, identity: true));
            DropTable("acco.LocalizedCategories");
            AddPrimaryKey("dbo.SupportedCultures", "Id");
        }
    }
}
