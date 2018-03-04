namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRegionsCreatorId : DbMigration
    {
        public override void Up()
        {
            DropIndex("geo.Regions", new[] { "CreatorId" });
            AlterColumn("geo.Regions", "CreatorId", c => c.Int(nullable: false));
            CreateIndex("geo.Regions", "CreatorId");
        }
        
        public override void Down()
        {
            DropIndex("geo.Regions", new[] { "CreatorId" });
            AlterColumn("geo.Regions", "CreatorId", c => c.Int());
            CreateIndex("geo.Regions", "CreatorId");
        }
    }
}
