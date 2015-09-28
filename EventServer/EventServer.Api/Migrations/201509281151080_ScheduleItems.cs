namespace EventServer.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScheduleItems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScheduleItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EventId = c.Guid(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScheduleItems", "EventId", "dbo.Events");
            DropIndex("dbo.ScheduleItems", new[] { "EventId" });
            DropTable("dbo.ScheduleItems");
        }
    }
}
