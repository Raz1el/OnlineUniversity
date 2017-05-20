namespace WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classrooms", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Classrooms", "Name");
        }
    }
}
