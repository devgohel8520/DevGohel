namespace DevGohel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderIdTdata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TDatas", "OrderId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TDatas", "OrderId");
        }
    }
}
