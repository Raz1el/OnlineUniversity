namespace WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lectures", "LectureCreator_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Lectures", "LectureCreator_Id");
            AddForeignKey("dbo.Lectures", "LectureCreator_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lectures", "LectureCreator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Lectures", new[] { "LectureCreator_Id" });
            DropColumn("dbo.Lectures", "LectureCreator_Id");
        }
    }
}
