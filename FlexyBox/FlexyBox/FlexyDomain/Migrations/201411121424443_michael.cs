namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class michael : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StepAnswers", "State", c => c.Int(nullable: false));
            CreateIndex("dbo.StepAnswers", "CustomerFlowId");
            AddForeignKey("dbo.StepAnswers", "CustomerFlowId", "dbo.CustomerFlows", "Id", cascadeDelete: true);
            DropColumn("dbo.StepAnswers", "QuestionAnswer");
            DropColumn("dbo.StepQuestions", "AnswerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StepQuestions", "AnswerId", c => c.Int(nullable: false));
            AddColumn("dbo.StepAnswers", "QuestionAnswer", c => c.Int(nullable: false));
            DropForeignKey("dbo.StepAnswers", "CustomerFlowId", "dbo.CustomerFlows");
            DropIndex("dbo.StepAnswers", new[] { "CustomerFlowId" });
            DropColumn("dbo.StepAnswers", "State");
        }
    }
}
