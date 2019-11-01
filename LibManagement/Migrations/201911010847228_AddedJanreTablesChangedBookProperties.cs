namespace LibManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedJanreTablesChangedBookProperties : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Janres",
                c => new
                    {
                        JanreId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.JanreId);
            
            AddColumn("dbo.Books", "JanreId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "JanreId");
            AddForeignKey("dbo.Books", "JanreId", "dbo.Janres", "JanreId", cascadeDelete: true);
            DropColumn("dbo.Books", "JanreName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "JanreName", c => c.String(maxLength: 150));
            DropForeignKey("dbo.Books", "JanreId", "dbo.Janres");
            DropIndex("dbo.Books", new[] { "JanreId" });
            DropColumn("dbo.Books", "JanreId");
            DropTable("dbo.Janres");
        }
    }
}
