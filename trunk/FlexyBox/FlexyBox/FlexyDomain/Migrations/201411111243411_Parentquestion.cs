namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Parentquestion : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.StepQuestions", new[] { "Question_Id" });
            RenameColumn(table: "dbo.StepQuestions", name: "Question_Id", newName: "ChildQuestion_Id");
            AlterColumn("dbo.StepQuestions", "ChildQuestion_Id", c => c.Int(nullable: true));
            CreateIndex("dbo.StepQuestions", "ChildQuestion_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.StepQuestions", new[] { "ChildQuestion_Id" });
            AlterColumn("dbo.StepQuestions", "ChildQuestion_Id", c => c.Int());
            RenameColumn(table: "dbo.StepQuestions", name: "ChildQuestion_Id", newName: "Question_Id");
            CreateIndex("dbo.StepQuestions", "Question_Id");
        }
    }
}
