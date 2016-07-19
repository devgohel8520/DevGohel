namespace DevGohel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateNewDev : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        EmailId = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AuthorId);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        TopicId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Ename = c.String(nullable: false),
                        TopicType = c.Int(nullable: false),
                        BgColor = c.Int(nullable: false),
                        TxColor = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        LabelId = c.Long(nullable: false),
                        AuthorId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TopicId)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Labels", t => t.LabelId, cascadeDelete: true)
                .Index(t => t.LabelId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Labels",
                c => new
                    {
                        LabelId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        EName = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LabelId);
            
            CreateTable(
                "dbo.TDatas",
                c => new
                    {
                        TdataId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        TopicId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TdataId)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.Visitors",
                c => new
                    {
                        VisitorId = c.Long(nullable: false, identity: true),
                        IpAddress = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        TopicId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.VisitorId)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.Sponsers",
                c => new
                    {
                        SponserId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Body = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SponserId);
            
            CreateTable(
                "dbo.TDataFiles",
                c => new
                    {
                        TDataFileId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        FullName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TDataFileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visitors", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.TDatas", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.Topics", "LabelId", "dbo.Labels");
            DropForeignKey("dbo.Topics", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Visitors", new[] { "TopicId" });
            DropIndex("dbo.TDatas", new[] { "TopicId" });
            DropIndex("dbo.Topics", new[] { "AuthorId" });
            DropIndex("dbo.Topics", new[] { "LabelId" });
            DropTable("dbo.TDataFiles");
            DropTable("dbo.Sponsers");
            DropTable("dbo.Visitors");
            DropTable("dbo.TDatas");
            DropTable("dbo.Labels");
            DropTable("dbo.Topics");
            DropTable("dbo.Authors");
        }
    }
}
