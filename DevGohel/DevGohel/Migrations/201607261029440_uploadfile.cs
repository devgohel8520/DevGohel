namespace DevGohel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uploadfile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UploadFiles",
                c => new
                    {
                        UploadFileId = c.Long(nullable: false, identity: true),
                        FileName = c.String(nullable: false),
                        FExtension = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UploadFileId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UploadFiles");
        }
    }
}
