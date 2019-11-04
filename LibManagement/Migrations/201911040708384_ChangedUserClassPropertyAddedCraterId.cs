namespace LibManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedUserClassPropertyAddedCraterId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CreaterId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "CreaterId");
        }
    }
}
