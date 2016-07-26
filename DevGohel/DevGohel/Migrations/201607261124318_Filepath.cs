namespace DevGohel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filepath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UploadFiles", "FilePath", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UploadFiles", "FilePath");
        }
    }
}
