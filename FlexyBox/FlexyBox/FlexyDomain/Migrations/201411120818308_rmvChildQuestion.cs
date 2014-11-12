namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rmvChildQuestion : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.StepQuestions", new[] { "ChildQuestion_Id" });
            RenameColumn(table: "dbo.StepQuestions", name: "ChildQuestion_Id", newName: "Question_Id");
            AlterColumn("dbo.StepQuestions", "Question_Id", c => c.Int());
            CreateIndex("dbo.StepQuestions", "Question_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.StepQuestions", new[] { "Question_Id" });
            AlterColumn("dbo.StepQuestions", "Question_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.StepQuestions", name: "Question_Id", newName: "ChildQuestion_Id");
            CreateIndex("dbo.StepQuestions", "ChildQuestion_Id");
        }
    }
}
