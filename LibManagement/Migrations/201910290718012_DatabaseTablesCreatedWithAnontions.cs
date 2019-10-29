namespace LibManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseTablesCreatedWithAnontions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookOrders",
                c => new
                    {
                        BookOrderId = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookOrderId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.BookId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        AutorName = c.String(nullable: false, maxLength: 100),
                        JanreName = c.String(maxLength: 150),
                        TotalPiece = c.Int(nullable: false),
                        SalePrice = c.Decimal(nullable: false, storeType: "money"),
                        RentPrice = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.BookId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        CreatedAt = c.DateTime(nullable: false, storeType: "date"),
                        Deadline = c.DateTime(nullable: false, storeType: "date"),
                        TotalRentPrice = c.Decimal(nullable: false, storeType: "money"),
                        FineForLate = c.Decimal(nullable: false, storeType: "money"),
                        Status = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        IdCode = c.String(nullable: false, maxLength: 50),
                        FullName = c.String(nullable: false, maxLength: 100),
                        PhoneNumber = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.BookOrders", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.BookOrders", "BookId", "dbo.Books");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.BookOrders", new[] { "OrderId" });
            DropIndex("dbo.BookOrders", new[] { "BookId" });
            DropTable("dbo.Users");
            DropTable("dbo.Customers");
            DropTable("dbo.Orders");
            DropTable("dbo.Books");
            DropTable("dbo.BookOrders");
        }
    }
}
