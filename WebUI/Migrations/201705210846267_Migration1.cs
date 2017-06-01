namespace WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Solutions", "Classroom_Id", "dbo.Classrooms");
            DropIndex("dbo.Solutions", new[] { "Classroom_Id" });
            DropColumn("dbo.Solutions", "Classroom_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Solutions", "Classroom_Id", c => c.Int());
            CreateIndex("dbo.Solutions", "Classroom_Id");
            AddForeignKey("dbo.Solutions", "Classroom_Id", "dbo.Classrooms", "Id");
        }
    }
}
