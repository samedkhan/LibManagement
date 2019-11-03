namespace LibManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedBookPropertyADdedInLibrary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "InLibrary", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "InLibrary");
        }
    }
}
