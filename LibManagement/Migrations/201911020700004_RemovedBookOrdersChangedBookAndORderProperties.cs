namespace LibManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedBookOrdersChangedBookAndORderProperties : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BookOrders", "BookId", "dbo.Books");
            DropForeignKey("dbo.BookOrders", "OrderId", "dbo.Orders");
            DropIndex("dbo.BookOrders", new[] { "BookId" });
            DropIndex("dbo.BookOrders", new[] { "OrderId" });
            AddColumn("dbo.Orders", "BookId", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "BookId");
            AddForeignKey("dbo.Orders", "BookId", "dbo.Books", "BookId", cascadeDelete: true);
            DropTable("dbo.BookOrders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BookOrders",
                c => new
                    {
                        BookOrderId = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookOrderId);
            
            DropForeignKey("dbo.Orders", "BookId", "dbo.Books");
            DropIndex("dbo.Orders", new[] { "BookId" });
            DropColumn("dbo.Orders", "BookId");
            CreateIndex("dbo.BookOrders", "OrderId");
            CreateIndex("dbo.BookOrders", "BookId");
            AddForeignKey("dbo.BookOrders", "OrderId", "dbo.Orders", "OrderId", cascadeDelete: true);
            AddForeignKey("dbo.BookOrders", "BookId", "dbo.Books", "BookId", cascadeDelete: true);
        }
    }
}
