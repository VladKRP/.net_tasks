namespace EFORMSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "SupplierID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            AlterColumn("dbo.Employees", "ReportsTo", c => c.Int());
            AlterColumn("dbo.Products", "SupplierID", c => c.Int());
            AlterColumn("dbo.Products", "CategoryID", c => c.Int());
            CreateIndex("dbo.Products", "SupplierID");
            CreateIndex("dbo.Products", "CategoryID");
            AddForeignKey("dbo.Products", "CategoryID", "dbo.Categories", "CategoryID");
            AddForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers", "SupplierID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropIndex("dbo.Products", new[] { "SupplierID" });
            AlterColumn("dbo.Products", "CategoryID", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "SupplierID", c => c.Int(nullable: false));
            AlterColumn("dbo.Employees", "ReportsTo", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "CategoryID");
            CreateIndex("dbo.Products", "SupplierID");
            AddForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers", "SupplierID", cascadeDelete: true);
            AddForeignKey("dbo.Products", "CategoryID", "dbo.Categories", "CategoryID", cascadeDelete: true);
        }
    }
}
