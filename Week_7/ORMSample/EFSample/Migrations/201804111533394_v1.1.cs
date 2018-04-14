namespace EFORMSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeCreditCards",
                c => new
                    {
                        CardNumber = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        CardHolder = c.String(),
                        ExpireDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CardNumber)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeCreditCards", "EmployeeID", "dbo.Employees");
            DropIndex("dbo.EmployeeCreditCards", new[] { "EmployeeID" });
            DropTable("dbo.EmployeeCreditCards");
        }
    }
}
