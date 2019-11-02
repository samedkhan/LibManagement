namespace LibManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedCustomerClassProperties : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "FullName", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.Customers", "IdCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "IdCode", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Customers", "FullName", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
