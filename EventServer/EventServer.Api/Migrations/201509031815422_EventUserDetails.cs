namespace EventServer.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventUserDetails : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Events", new[] { "OwnerId" });
            AddColumn("dbo.EventUsers", "Owner", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventUsers", "Attending", c => c.Int(nullable: false));
            DropColumn("dbo.Events", "OwnerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "OwnerId", c => c.String(maxLength: 128));
            DropColumn("dbo.EventUsers", "Attending");
            DropColumn("dbo.EventUsers", "Owner");
            CreateIndex("dbo.Events", "OwnerId");
            AddForeignKey("dbo.Events", "OwnerId", "dbo.AspNetUsers", "Id");
        }
    }
}
