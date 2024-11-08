namespace Advanced.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatHang : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DatHangs",
                c => new
                    {
                        MaDonHang = c.String(nullable: false, maxLength: 128),
                        NgayMua = c.DateTime(nullable: false),
                        TongTien = c.Single(nullable: false),
                        UserId = c.String(maxLength: 128),
                        TrangThai = c.Boolean(nullable: false),
                        kh_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MaDonHang)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Khoahocs", t => t.kh_id, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.kh_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DatHangs", "kh_id", "dbo.Khoahocs");
            DropForeignKey("dbo.DatHangs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.DatHangs", new[] { "kh_id" });
            DropIndex("dbo.DatHangs", new[] { "UserId" });
            DropTable("dbo.DatHangs");
        }
    }
}
