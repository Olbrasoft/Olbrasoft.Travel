namespace Olbrasoft.Travel.EAN.Import.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attributes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attributes",
                c => new
                    {
                        AttributeID = c.Int(nullable: false),
                        LanguageCode = c.String(nullable: false, maxLength: 5),
                        AttributeDesc = c.String(maxLength: 255),
                        Type = c.String(maxLength: 15),
                        SubType = c.String(maxLength: 15),
                    })
                .PrimaryKey(t => t.AttributeID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Attributes");
        }
    }
}
