namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerFlows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StepAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        EmployeeId = c.Int(nullable: false),
                        TimeChanged = c.DateTime(nullable: false),
                        IsLog = c.Boolean(nullable: false),
                        QuestionAnswer = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StepQuestions", t => t.Question_Id)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.StepQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StepGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StepAnswers", "Question_Id", "dbo.StepQuestions");
            DropIndex("dbo.StepAnswers", new[] { "Question_Id" });
            DropTable("dbo.StepGroups");
            DropTable("dbo.StepQuestions");
            DropTable("dbo.StepAnswers");
            DropTable("dbo.CustomerFlows");
        }
    }
}
