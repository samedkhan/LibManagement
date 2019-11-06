namespace LibManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCreatedATperopertyCustomerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "CreatedAt", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "CreatedAt");
        }
    }
}
