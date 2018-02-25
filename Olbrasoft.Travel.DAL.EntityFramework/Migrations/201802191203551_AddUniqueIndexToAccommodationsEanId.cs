namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueIndexToAccommodationsEanId : DbMigration
    {
        public override void Up()
        {
            CreateIndex("acco.Accommodations", "EanId", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("acco.Accommodations", new[] { "EanId" });
        }
    }
}
