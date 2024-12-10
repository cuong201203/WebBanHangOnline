﻿namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProduct3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "SoldQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "SoldQuantity");
        }
    }
}