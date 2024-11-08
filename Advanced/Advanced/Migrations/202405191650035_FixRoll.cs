namespace Advanced.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRoll : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RollOuts", "date", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RollOuts", "date");
        }
    }
}
