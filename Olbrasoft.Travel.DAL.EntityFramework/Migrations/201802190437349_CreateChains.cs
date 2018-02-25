namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateChains : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "acco.Chains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        EanId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.EanId, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("acco.Chains", new[] { "EanId" });
            DropTable("acco.Chains");
        }
    }
}
