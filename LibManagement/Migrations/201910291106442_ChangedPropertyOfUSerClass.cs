namespace LibManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedPropertyOfUSerClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "AdminOrUser", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "AdminOrUser");
        }
    }
}
