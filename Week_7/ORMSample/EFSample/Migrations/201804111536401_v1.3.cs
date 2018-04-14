namespace EFORMSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v13 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Regions", newName: "Region");
            AddColumn("dbo.Customers", "EstablishDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "EstablishDate");
            RenameTable(name: "dbo.Region", newName: "Regions");
        }
    }
}
