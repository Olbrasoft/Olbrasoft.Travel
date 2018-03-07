namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePointsOfInterestEanRegionIdToNotNull : DbMigration
    {
        public override void Up()
        {
            DropIndex("geo.PointsOfInterest", new[] { "EanRegionId" });
            AlterColumn("geo.PointsOfInterest", "EanRegionId", c => c.Long(nullable: false));
            CreateIndex("geo.PointsOfInterest", "EanRegionId", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("geo.PointsOfInterest", new[] { "EanRegionId" });
            AlterColumn("geo.PointsOfInterest", "EanRegionId", c => c.Long());
            CreateIndex("geo.PointsOfInterest", "EanRegionId", unique: true);
        }
    }
}
