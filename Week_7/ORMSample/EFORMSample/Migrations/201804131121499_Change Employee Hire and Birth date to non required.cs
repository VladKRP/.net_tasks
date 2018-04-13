namespace EFORMSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEmployeeHireandBirthdatetononrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "BirthDate", c => c.DateTime());
            AlterColumn("dbo.Employees", "HireDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "HireDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Employees", "BirthDate", c => c.DateTime(nullable: false));
        }
    }
}
