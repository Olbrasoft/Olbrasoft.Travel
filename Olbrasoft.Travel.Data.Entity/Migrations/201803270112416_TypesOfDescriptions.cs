namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class TypesOfDescriptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.TypesOfDescriptions",
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
            DropForeignKey("acco.TypesOfDescriptions", "CreatorId", "dbo.Users");
            DropIndex("acco.TypesOfDescriptions", new[] { "CreatorId" });
            DropIndex("acco.TypesOfDescriptions", new[] { "Name" });
            DropTable("acco.TypesOfDescriptions");
        }
    }
}
