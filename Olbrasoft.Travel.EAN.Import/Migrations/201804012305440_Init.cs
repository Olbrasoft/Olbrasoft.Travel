namespace Olbrasoft.Travel.EAN.Import.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DevelopmentRoomTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EANHotelID = c.Int(nullable: false),
                        RoomTypeID = c.Int(nullable: false),
                        LanguageCode = c.String(maxLength: 5),
                        RoomTypeImage = c.String(maxLength: 256),
                        RoomTypeName = c.String(maxLength: 200),
                        RoomTypeDescription = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DevelopmentRoomTypes");
        }
    }
}
