namespace DevGohel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TDataTypeEnum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TDatas", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TDatas", "Type");
        }
    }
}
