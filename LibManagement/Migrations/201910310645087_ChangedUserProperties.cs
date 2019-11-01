﻿namespace LibManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedUserProperties : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "FullName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Users", "Username", c => c.String(maxLength: 50));
            AlterColumn("dbo.Users", "Password", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Users", "FullName", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
