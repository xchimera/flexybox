namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "StepQuestion_Id", "dbo.StepQuestions");
            DropIndex("dbo.Products", new[] { "StepQuestion_Id" });
            DropColumn("dbo.Products", "StepQuestion_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "StepQuestion_Id", c => c.Int());
            CreateIndex("dbo.Products", "StepQuestion_Id");
            AddForeignKey("dbo.Products", "StepQuestion_Id", "dbo.StepQuestions", "Id");
        }
    }
}
