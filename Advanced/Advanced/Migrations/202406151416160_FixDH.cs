namespace Advanced.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDH : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DatHangs");
            AlterColumn("dbo.DatHangs", "MaDonHang", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.DatHangs", "MaDonHang");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.DatHangs");
            AlterColumn("dbo.DatHangs", "MaDonHang", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.DatHangs", "MaDonHang");
        }
    }
}
