namespace DevGohel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtraTextBoxForTData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TDatas", "ExtraText", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TDatas", "ExtraText");
        }
    }
}
