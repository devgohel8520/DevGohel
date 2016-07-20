namespace DevGohel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TDAtaFile : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sponsers", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Sponsers", "Body", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sponsers", "Body", c => c.String());
            AlterColumn("dbo.Sponsers", "Title", c => c.String());
        }
    }
}
