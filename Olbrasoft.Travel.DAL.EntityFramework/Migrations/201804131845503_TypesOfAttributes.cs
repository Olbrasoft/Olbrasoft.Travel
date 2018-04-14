namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class TypesOfAttributes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.TypesOfAttributes",
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
            DropForeignKey("acco.TypesOfAttributes", "CreatorId", "dbo.Users");
            DropIndex("acco.TypesOfAttributes", new[] { "CreatorId" });
            DropIndex("acco.TypesOfAttributes", new[] { "Name" });
            DropTable("acco.TypesOfAttributes");
        }
    }
}
