namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPersistensy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerFlows", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.StepQuestions", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.StepGroups", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StepGroups", "IsDeleted");
            DropColumn("dbo.StepQuestions", "IsDeleted");
            DropColumn("dbo.CustomerFlows", "IsDeleted");
        }
    }
}
