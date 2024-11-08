namespace Advanced.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class A : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ca_id = c.Int(nullable: false, identity: true),
                        ca_name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ca_id);
            
            CreateTable(
                "dbo.Khoahocs",
                c => new
                    {
                        kh_id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        name = c.String(),
                        category_id = c.Int(nullable: false),
                        Content = c.String(nullable: false),
                        Image = c.Binary(),
                        Price = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.kh_id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Categories", t => t.category_id, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.category_id);
            
            CreateTable(
                "dbo.ClassMembers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        lophoc_id = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Lophocs", t => t.lophoc_id, cascadeDelete: true)
                .Index(t => t.lophoc_id)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Lophocs",
                c => new
                    {
                        class_id = c.Int(nullable: false, identity: true),
                        class_name = c.String(),
                        content = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.class_id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Lichhocs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        lophoc_id = c.Int(nullable: false),
                        Ngayhoc1 = c.String(),
                        Tiet_1 = c.String(),
                        Ngayhoc2 = c.String(),
                        Tiet_2 = c.String(),
                        Ngayhoc3 = c.String(),
                        Tiet_3 = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Lophocs", t => t.lophoc_id, cascadeDelete: true)
                .Index(t => t.lophoc_id);
            
            CreateTable(
                "dbo.Main_Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        post_id = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        comments = c.String(),
                        DateComment = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Post_Post", t => t.post_id, cascadeDelete: true)
                .Index(t => t.post_id)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Post_Post",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        UserId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Sub_Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        main_id = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        comments = c.String(),
                        DateComment = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Main_Comment", t => t.main_id, cascadeDelete: true)
                .Index(t => t.main_id)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Marks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        lophoc_id = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Diem1 = c.Single(nullable: false),
                        Diem2 = c.Single(nullable: false),
                        DiemTK = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Lophocs", t => t.lophoc_id, cascadeDelete: true)
                .Index(t => t.lophoc_id)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.RollOuts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Lec_Content = c.String(nullable: false),
                        date = c.DateTime(),
                        excuted = c.Boolean(),
                        UserId = c.String(maxLength: 128),
                        lophoc_id = c.Int(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Lophocs", t => t.lophoc_id, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.lophoc_id);
            
            AddColumn("dbo.AspNetUsers", "Fullname", c => c.String());
            AddColumn("dbo.AspNetUsers", "Age", c => c.String());
            AddColumn("dbo.AspNetUsers", "Main_subject", c => c.String());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "BirtDate", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "Image", c => c.String());
            AddColumn("dbo.AspNetUsers", "ShortDesc", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RollOuts", "lophoc_id", "dbo.Lophocs");
            DropForeignKey("dbo.RollOuts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Marks", "lophoc_id", "dbo.Lophocs");
            DropForeignKey("dbo.Marks", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Sub_Comment", "main_id", "dbo.Main_Comment");
            DropForeignKey("dbo.Sub_Comment", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Main_Comment", "post_id", "dbo.Post_Post");
            DropForeignKey("dbo.Post_Post", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Main_Comment", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ClassMembers", "lophoc_id", "dbo.Lophocs");
            DropForeignKey("dbo.Lichhocs", "lophoc_id", "dbo.Lophocs");
            DropForeignKey("dbo.Lophocs", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ClassMembers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Khoahocs", "category_id", "dbo.Categories");
            DropForeignKey("dbo.Khoahocs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.RollOuts", new[] { "lophoc_id" });
            DropIndex("dbo.RollOuts", new[] { "UserId" });
            DropIndex("dbo.Marks", new[] { "UserId" });
            DropIndex("dbo.Marks", new[] { "lophoc_id" });
            DropIndex("dbo.Sub_Comment", new[] { "UserId" });
            DropIndex("dbo.Sub_Comment", new[] { "main_id" });
            DropIndex("dbo.Post_Post", new[] { "UserId" });
            DropIndex("dbo.Main_Comment", new[] { "UserId" });
            DropIndex("dbo.Main_Comment", new[] { "post_id" });
            DropIndex("dbo.Lichhocs", new[] { "lophoc_id" });
            DropIndex("dbo.Lophocs", new[] { "UserId" });
            DropIndex("dbo.ClassMembers", new[] { "UserId" });
            DropIndex("dbo.ClassMembers", new[] { "lophoc_id" });
            DropIndex("dbo.Khoahocs", new[] { "category_id" });
            DropIndex("dbo.Khoahocs", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "ShortDesc");
            DropColumn("dbo.AspNetUsers", "Image");
            DropColumn("dbo.AspNetUsers", "BirtDate");
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "Main_subject");
            DropColumn("dbo.AspNetUsers", "Age");
            DropColumn("dbo.AspNetUsers", "Fullname");
            DropTable("dbo.RollOuts");
            DropTable("dbo.Marks");
            DropTable("dbo.Sub_Comment");
            DropTable("dbo.Post_Post");
            DropTable("dbo.Main_Comment");
            DropTable("dbo.Lichhocs");
            DropTable("dbo.Lophocs");
            DropTable("dbo.ClassMembers");
            DropTable("dbo.Khoahocs");
            DropTable("dbo.Categories");
        }
    }
}
