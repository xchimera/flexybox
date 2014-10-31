namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Crap : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StepQuestions", "Question_Id", c => c.Int());
            AddColumn("dbo.StepQuestions", "StepGroup_Id", c => c.Int());
            CreateIndex("dbo.StepQuestions", "Question_Id");
            CreateIndex("dbo.StepQuestions", "StepGroup_Id");
            AddForeignKey("dbo.StepQuestions", "Question_Id", "dbo.StepQuestions", "Id");
            AddForeignKey("dbo.StepQuestions", "StepGroup_Id", "dbo.StepGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StepQuestions", "StepGroup_Id", "dbo.StepGroups");
            DropForeignKey("dbo.StepQuestions", "Question_Id", "dbo.StepQuestions");
            DropIndex("dbo.StepQuestions", new[] { "StepGroup_Id" });
            DropIndex("dbo.StepQuestions", new[] { "Question_Id" });
            DropColumn("dbo.StepQuestions", "StepGroup_Id");
            DropColumn("dbo.StepQuestions", "Question_Id");
        }
    }
}
