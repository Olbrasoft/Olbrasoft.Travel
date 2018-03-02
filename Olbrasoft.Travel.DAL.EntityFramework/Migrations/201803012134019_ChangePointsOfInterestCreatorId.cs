namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePointsOfInterestCreatorId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("geo.PointsOfInterest", "CreatorId", "dbo.Users");
            DropIndex("geo.PointsOfInterest", new[] { "CreatorId" });
            AlterColumn("geo.PointsOfInterest", "CreatorId", c => c.Int(nullable: false));
            CreateIndex("geo.PointsOfInterest", "CreatorId");
            AddForeignKey("geo.PointsOfInterest", "CreatorId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("geo.PointsOfInterest", "CreatorId", "dbo.Users");
            DropIndex("geo.PointsOfInterest", new[] { "CreatorId" });
            AlterColumn("geo.PointsOfInterest", "CreatorId", c => c.Int());
            CreateIndex("geo.PointsOfInterest", "CreatorId");
            AddForeignKey("geo.PointsOfInterest", "CreatorId", "dbo.Users", "Id");
        }
    }
}
