namespace Advanced.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lophocs", "Image", c => c.String());
            AddColumn("dbo.Post_Post", "Image", c => c.String());
            AddColumn("dbo.RollOuts", "day", c => c.String());
            AlterColumn("dbo.Khoahocs", "Image", c => c.String());
            DropColumn("dbo.RollOuts", "date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RollOuts", "date", c => c.DateTime());
            AlterColumn("dbo.Khoahocs", "Image", c => c.Binary());
            DropColumn("dbo.RollOuts", "day");
            DropColumn("dbo.Post_Post", "Image");
            DropColumn("dbo.Lophocs", "Image");
        }
    }
}
