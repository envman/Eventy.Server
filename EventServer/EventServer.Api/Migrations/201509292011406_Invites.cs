namespace EventServer.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invites",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EventId = c.Guid(nullable: false),
                        InvitedById = c.String(maxLength: 128),
                        CreatedUserId = c.String(maxLength: 128),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedUserId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.InvitedById)
                .Index(t => t.EventId)
                .Index(t => t.InvitedById)
                .Index(t => t.CreatedUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invites", "InvitedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Invites", "EventId", "dbo.Events");
            DropForeignKey("dbo.Invites", "CreatedUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Invites", new[] { "CreatedUserId" });
            DropIndex("dbo.Invites", new[] { "InvitedById" });
            DropIndex("dbo.Invites", new[] { "EventId" });
            DropTable("dbo.Invites");
        }
    }
}
