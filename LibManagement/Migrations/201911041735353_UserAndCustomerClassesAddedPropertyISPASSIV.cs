namespace LibManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAndCustomerClassesAddedPropertyISPASSIV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "IsPassiv", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "IsAdmin", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "IsPassiv", c => c.Boolean(nullable: false));
            DropColumn("dbo.Users", "AdminOrUser");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "AdminOrUser", c => c.Boolean(nullable: false));
            DropColumn("dbo.Users", "IsPassiv");
            DropColumn("dbo.Users", "IsAdmin");
            DropColumn("dbo.Customers", "IsPassiv");
        }
    }
}
