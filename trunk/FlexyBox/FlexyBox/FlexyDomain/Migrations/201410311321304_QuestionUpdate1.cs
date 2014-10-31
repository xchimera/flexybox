namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionUpdate1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.StepQuestions", name: "Group_Id", newName: "StepGroup_Id");
            RenameIndex(table: "dbo.StepQuestions", name: "IX_Group_Id", newName: "IX_StepGroup_Id");
            AddColumn("dbo.StepQuestions", "GroupId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StepQuestions", "GroupId");
            RenameIndex(table: "dbo.StepQuestions", name: "IX_StepGroup_Id", newName: "IX_Group_Id");
            RenameColumn(table: "dbo.StepQuestions", name: "StepGroup_Id", newName: "Group_Id");
        }
    }
}
