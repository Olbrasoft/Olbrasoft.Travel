namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Attributes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.Attributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeOfAttributeId = c.Int(nullable: false),
                        SubTypeOfAttributeId = c.Int(nullable: false),
                        EanId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        DateAndTimeOfCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorId, cascadeDelete: true)
                .ForeignKey("acco.SubTypesOfAttributes", t => t.SubTypeOfAttributeId)
                .ForeignKey("acco.TypesOfAttributes", t => t.TypeOfAttributeId)
                .Index(t => t.TypeOfAttributeId)
                .Index(t => t.SubTypeOfAttributeId)
                .Index(t => t.CreatorId);
        }
        
        public override void Down()
        {
            DropForeignKey("acco.Attributes", "TypeOfAttributeId", "acco.TypesOfAttributes");
            DropForeignKey("acco.Attributes", "SubTypeOfAttributeId", "acco.SubTypesOfAttributes");
            DropForeignKey("acco.Attributes", "CreatorId", "dbo.Users");
            DropIndex("acco.Attributes", new[] { "CreatorId" });
            DropIndex("acco.Attributes", new[] { "SubTypeOfAttributeId" });
            DropIndex("acco.Attributes", new[] { "TypeOfAttributeId" });
            DropTable("acco.Attributes");
        }
    }
}
