namespace LibManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAllModelsPropertyDigitForSum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "DigitForSum", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "DigitForSum", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "DigitForSum", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "DigitForSum", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "DigitForSum");
            DropColumn("dbo.Customers", "DigitForSum");
            DropColumn("dbo.Orders", "DigitForSum");
            DropColumn("dbo.Books", "DigitForSum");
        }
    }
}
