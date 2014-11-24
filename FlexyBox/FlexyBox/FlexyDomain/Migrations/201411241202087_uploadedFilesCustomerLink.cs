namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uploadedFilesCustomerLink : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.UploadedFiles", name: "CustomerFlow_Id", newName: "Customer_Id");
            RenameIndex(table: "dbo.UploadedFiles", name: "IX_CustomerFlow_Id", newName: "IX_Customer_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.UploadedFiles", name: "IX_Customer_Id", newName: "IX_CustomerFlow_Id");
            RenameColumn(table: "dbo.UploadedFiles", name: "Customer_Id", newName: "CustomerFlow_Id");
        }
    }
}
