namespace LibManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedBookProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "InOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "InOrder");
        }
    }
}
