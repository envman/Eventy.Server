namespace EventServer.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "OwnerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Events", "OwnerId");
            AddForeignKey("dbo.Events", "OwnerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Events", new[] { "OwnerId" });
            DropColumn("dbo.Events", "OwnerId");
        }
    }
}
