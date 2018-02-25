namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateSubClassesOfRegions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.SubClassesOfRegions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("geo.SubClassesOfRegions", new[] { "Name" });
            DropTable("geo.SubClassesOfRegions");
        }
    }
}
