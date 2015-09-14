namespace EventServer.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Events", "ImageId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "ImageId");
            DropTable("dbo.Images");
        }
    }
}
