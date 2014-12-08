namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questionvisibility1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StepQuestions", "Visibility_Id", c => c.Int());
            CreateIndex("dbo.StepQuestions", "Visibility_Id");
            AddForeignKey("dbo.StepQuestions", "Visibility_Id", "dbo.QuestionVisibilities", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StepQuestions", "Visibility_Id", "dbo.QuestionVisibilities");
            DropIndex("dbo.StepQuestions", new[] { "Visibility_Id" });
            DropColumn("dbo.StepQuestions", "Visibility_Id");
        }
    }
}
