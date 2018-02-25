namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChageDescriptionsTypesToTypesOfDescriptions : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "acco.Descriptions", name: "DescriptionTypeId", newName: "TypeOfDescriptionId");
            RenameIndex(table: "acco.Descriptions", name: "IX_DescriptionTypeId", newName: "IX_TypeOfDescriptionId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "acco.Descriptions", name: "IX_TypeOfDescriptionId", newName: "IX_DescriptionTypeId");
            RenameColumn(table: "acco.Descriptions", name: "TypeOfDescriptionId", newName: "DescriptionTypeId");
        }
    }
}
