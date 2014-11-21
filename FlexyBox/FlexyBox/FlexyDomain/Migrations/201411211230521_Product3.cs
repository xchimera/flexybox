namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StepQuestions", "Product_Id", c => c.Int());
            CreateIndex("dbo.StepQuestions", "Product_Id");
            AddForeignKey("dbo.StepQuestions", "Product_Id", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StepQuestions", "Product_Id", "dbo.Products");
            DropIndex("dbo.StepQuestions", new[] { "Product_Id" });
            DropColumn("dbo.StepQuestions", "Product_Id");
        }
    }
}
