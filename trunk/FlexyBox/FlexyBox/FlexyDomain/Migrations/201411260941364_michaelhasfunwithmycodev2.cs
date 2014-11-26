namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class michaelhasfunwithmycodev2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StepAnswers", "CustomerFlowId", "dbo.CustomerFlows");
            DropIndex("dbo.StepAnswers", new[] { "CustomerFlowId" });
            RenameColumn(table: "dbo.StepAnswers", name: "CustomerFlowId", newName: "CustomerFlow_Id");
            AddColumn("dbo.StepAnswers", "Question_Id", c => c.Int());
            AlterColumn("dbo.StepAnswers", "CustomerFlow_Id", c => c.Int());
            CreateIndex("dbo.StepAnswers", "CustomerFlow_Id");
            CreateIndex("dbo.StepAnswers", "Question_Id");
            AddForeignKey("dbo.StepAnswers", "Question_Id", "dbo.StepQuestions", "Id"); 
            AddForeignKey("dbo.StepAnswers", "CustomerFlow_Id", "dbo.CustomerFlows", "Id");
            DropColumn("dbo.StepAnswers", "QuestionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StepAnswers", "QuestionId", c => c.Int(nullable: false));
            DropForeignKey("dbo.StepAnswers", "CustomerFlow_Id", "dbo.CustomerFlows");
            DropForeignKey("dbo.StepAnswers", "Question_Id", "dbo.StepQuestions");
            DropIndex("dbo.StepAnswers", new[] { "Question_Id" });
            DropIndex("dbo.StepAnswers", new[] { "CustomerFlow_Id" });
            AlterColumn("dbo.StepAnswers", "CustomerFlow_Id", c => c.Int(nullable: false));
            DropColumn("dbo.StepAnswers", "Question_Id");
            RenameColumn(table: "dbo.StepAnswers", name: "CustomerFlow_Id", newName: "CustomerFlowId");
            CreateIndex("dbo.StepAnswers", "CustomerFlowId");
            AddForeignKey("dbo.StepAnswers", "CustomerFlowId", "dbo.CustomerFlows", "Id", cascadeDelete: true);
        }
    }
}
