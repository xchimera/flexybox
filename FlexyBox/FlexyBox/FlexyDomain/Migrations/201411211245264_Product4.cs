namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductCustomerFlows",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        CustomerFlow_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.CustomerFlow_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerFlows", t => t.CustomerFlow_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.CustomerFlow_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductCustomerFlows", "CustomerFlow_Id", "dbo.CustomerFlows");
            DropForeignKey("dbo.ProductCustomerFlows", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductCustomerFlows", new[] { "CustomerFlow_Id" });
            DropIndex("dbo.ProductCustomerFlows", new[] { "Product_Id" });
            DropTable("dbo.ProductCustomerFlows");
        }
    }
}
