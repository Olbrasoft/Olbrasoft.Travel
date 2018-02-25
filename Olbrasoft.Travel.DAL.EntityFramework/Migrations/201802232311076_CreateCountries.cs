namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCountries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "geo.Countries",
                c => new
                    {
                        RegionId = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 2),
                    })
                .PrimaryKey(t => t.RegionId)
                .Index(t => t.Code, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("geo.Countries", new[] { "Code" });
            DropTable("geo.Countries");
        }
    }
}
