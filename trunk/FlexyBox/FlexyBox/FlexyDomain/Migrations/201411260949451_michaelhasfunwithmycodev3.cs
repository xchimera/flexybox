namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class michaelhasfunwithmycodev3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StepAnswers", "Question_Id", "dbo.StepQuestions");
            DropIndex("dbo.StepAnswers", new[] { "Question_Id" });
            AddColumn("dbo.StepAnswers", "QuestionId", c => c.Int(nullable: false));
            DropColumn("dbo.StepAnswers", "Question_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StepAnswers", "Question_Id", c => c.Int());
            DropColumn("dbo.StepAnswers", "QuestionId");
            CreateIndex("dbo.StepAnswers", "Question_Id");
            AddForeignKey("dbo.StepAnswers", "Question_Id", "dbo.StepQuestions", "Id");
        }
    }
}
