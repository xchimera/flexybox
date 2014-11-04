namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class michaelRedone : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StepAnswers", "Question_Id", "dbo.StepQuestions");
            DropIndex("dbo.StepAnswers", new[] { "Question_Id" });
            RenameColumn(table: "dbo.StepQuestions", name: "Group_Id", newName: "StepGroup_Id");
            RenameIndex(table: "dbo.StepQuestions", name: "IX_Group_Id", newName: "IX_StepGroup_Id");
            AddColumn("dbo.StepAnswers", "QuestionId", c => c.Int(nullable: false));
            AddColumn("dbo.StepQuestions", "GroupId", c => c.Int(nullable: false));
            DropColumn("dbo.StepAnswers", "Question_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StepAnswers", "Question_Id", c => c.Int());
            DropColumn("dbo.StepQuestions", "GroupId");
            DropColumn("dbo.StepAnswers", "QuestionId");
            RenameIndex(table: "dbo.StepQuestions", name: "IX_StepGroup_Id", newName: "IX_Group_Id");
            RenameColumn(table: "dbo.StepQuestions", name: "StepGroup_Id", newName: "Group_Id");
            CreateIndex("dbo.StepAnswers", "Question_Id");
            AddForeignKey("dbo.StepAnswers", "Question_Id", "dbo.StepQuestions", "Id");
        }
    }
}
