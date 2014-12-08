namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questionvisibility : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionVisibilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StepQuestions", t => t.Question_Id)
                .Index(t => t.Question_Id);
            
            AddColumn("dbo.StepQuestions", "QuestionVisibility_Id", c => c.Int());
            CreateIndex("dbo.StepQuestions", "QuestionVisibility_Id");
            AddForeignKey("dbo.StepQuestions", "QuestionVisibility_Id", "dbo.QuestionVisibilities", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StepQuestions", "QuestionVisibility_Id", "dbo.QuestionVisibilities");
            DropForeignKey("dbo.QuestionVisibilities", "Question_Id", "dbo.StepQuestions");
            DropIndex("dbo.StepQuestions", new[] { "QuestionVisibility_Id" });
            DropIndex("dbo.QuestionVisibilities", new[] { "Question_Id" });
            DropColumn("dbo.StepQuestions", "QuestionVisibility_Id");
            DropTable("dbo.QuestionVisibilities");
        }
    }
}
