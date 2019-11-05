namespace LibManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedPropertyBookAddedIsPassivproperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "isPassiv", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "isPassiv");
        }
    }
}
