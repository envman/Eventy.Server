namespace EventServer.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatMessage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PosterId = c.String(maxLength: 128),
                        EventId = c.Guid(nullable: false),
                        Message = c.String(),
                        PostTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.PosterId)
                .Index(t => t.PosterId)
                .Index(t => t.EventId);
            
            AddColumn("dbo.Events", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatMessages", "PosterId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChatMessages", "EventId", "dbo.Events");
            DropIndex("dbo.ChatMessages", new[] { "EventId" });
            DropIndex("dbo.ChatMessages", new[] { "PosterId" });
            DropColumn("dbo.Events", "Location");
            DropTable("dbo.ChatMessages");
        }
    }
}
