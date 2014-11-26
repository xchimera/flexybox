namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class michaelhasfunwithmycodev1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.StepQuestions", name: "StepGroup_Id", newName: "Group_Id");
            RenameIndex(table: "dbo.StepQuestions", name: "IX_StepGroup_Id", newName: "IX_Group_Id");
            DropColumn("dbo.StepQuestions", "GroupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StepQuestions", "GroupId", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.StepQuestions", name: "IX_Group_Id", newName: "IX_StepGroup_Id");
            RenameColumn(table: "dbo.StepQuestions", name: "Group_Id", newName: "StepGroup_Id");
        }
    }
}
