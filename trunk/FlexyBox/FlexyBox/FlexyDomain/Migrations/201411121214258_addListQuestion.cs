namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addListQuestion : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.StepQuestions", name: "Question_Id", newName: "Parent_Id");
            RenameIndex(table: "dbo.StepQuestions", name: "IX_Question_Id", newName: "IX_Parent_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.StepQuestions", name: "IX_Parent_Id", newName: "IX_Question_Id");
            RenameColumn(table: "dbo.StepQuestions", name: "Parent_Id", newName: "Question_Id");
        }
    }
}
