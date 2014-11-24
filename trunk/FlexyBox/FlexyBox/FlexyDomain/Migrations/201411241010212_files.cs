namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class files : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UploadedFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        File = c.Binary(),
                        IsDeleted = c.Boolean(nullable: false),
                        CustomerFlow_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerFlows", t => t.CustomerFlow_Id)
                .Index(t => t.CustomerFlow_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UploadedFiles", "CustomerFlow_Id", "dbo.CustomerFlows");
            DropIndex("dbo.UploadedFiles", new[] { "CustomerFlow_Id" });
            DropTable("dbo.UploadedFiles");
        }
    }
}
