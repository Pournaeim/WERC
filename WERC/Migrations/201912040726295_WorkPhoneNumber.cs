namespace WERC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkPhoneNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "WorkPhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "WorkPhoneNumber");
        }
    }
}
