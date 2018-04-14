namespace EFORMSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changemodelaccordingnonrequiredfields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "OrderDate", c => c.DateTime());
            AlterColumn("dbo.Orders", "RequiredDate", c => c.DateTime());
            AlterColumn("dbo.Orders", "ShippedDate", c => c.DateTime());
            AlterColumn("dbo.Orders", "ShipVia", c => c.Int());
            AlterColumn("dbo.Orders", "Freight", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Products", "UnitPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Products", "UnitsInStock", c => c.Int());
            AlterColumn("dbo.Products", "UnitsOnOrder", c => c.Int());
            AlterColumn("dbo.Products", "ReorderLevel", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ReorderLevel", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "UnitsOnOrder", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "UnitsInStock", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "UnitPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Orders", "Freight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Orders", "ShipVia", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "ShippedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "RequiredDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "OrderDate", c => c.DateTime(nullable: false));
        }
    }
}
