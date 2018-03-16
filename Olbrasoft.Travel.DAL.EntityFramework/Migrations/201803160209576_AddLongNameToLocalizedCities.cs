namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLongNameToLocalizedCities : DbMigration
    {
        public override void Up()
        {
            AddColumn("geo.LocalizedCities", "LongName", c => c.String(maxLength: 510));
        }
        
        public override void Down()
        {
            DropColumn("geo.LocalizedCities", "LongName");
        }
    }
}
