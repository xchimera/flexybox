namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerFlows", "Name", c => c.String());
            AddColumn("dbo.StepAnswers", "CustomerFlowId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StepAnswers", "CustomerFlowId");
            DropColumn("dbo.CustomerFlows", "Name");
        }
    }
}
