namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAccommodations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.Accommodations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 70),
                        Address = c.String(nullable: false, maxLength: 50),
                        Coordinates = c.Geography(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        ChainId = c.Int(nullable: false),
                        EanId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("acco.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("acco.Chains", t => t.ChainId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.ChainId);

            Sql("CREATE SPATIAL INDEX [IX_Accommodations_Coordinates] ON [acco].[Accommodations](Coordinates)");

        }
        
        public override void Down()
        {
           // Sql("DROP INDEX [IX_Accommodations_Coordinates] ON [acco].[Accommodations]");
            DropForeignKey("acco.Accommodations", "ChainId", "acco.Chains");
            DropForeignKey("acco.Accommodations", "CategoryId", "acco.Categories");
            DropIndex("acco.Accommodations", new[] { "ChainId" });
            DropIndex("acco.Accommodations", new[] { "CategoryId" });
            DropTable("acco.Accommodations");

           
        }
    }
}
