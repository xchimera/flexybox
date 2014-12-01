namespace FlexyDomain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameFile : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UploadedFiles", newName: "CustomerFiles");
            AddColumn("dbo.CustomerFiles", "FileType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerFiles", "FileType");
            RenameTable(name: "dbo.CustomerFiles", newName: "UploadedFiles");
        }
    }
}
