namespace DevGohel.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FeedbackData : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FeedBacks", "Body", c => c.String(nullable: false, maxLength: 500));
        }

        public override void Down()
        {
            AlterColumn("dbo.FeedBacks", "Body", c => c.String(maxLength: 500));
        }
    }
}
