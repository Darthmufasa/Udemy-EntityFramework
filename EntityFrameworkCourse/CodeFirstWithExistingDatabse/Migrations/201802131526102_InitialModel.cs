namespace CodeFirstWithExistingDatabse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Level = c.Int(nullable: false),
                        FullPrice = c.Single(nullable: false),
                        Author_ID = c.Int(),
                        Category_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.Category_ID)
                .ForeignKey("dbo.Authors", t => t.Author_ID)
                .Index(t => t.Author_ID)
                .Index(t => t.Category_ID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TagCourses",
                c => new
                    {
                        Course_ID = c.Int(nullable: false),
                        Tag_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Course_ID, t.Tag_ID })
                .ForeignKey("dbo.Courses", t => t.Course_ID, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.Tag_ID, cascadeDelete: true)
                .Index(t => t.Course_ID)
                .Index(t => t.Tag_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "Author_ID", "dbo.Authors");
            DropForeignKey("dbo.TagCourses", "Tag_ID", "dbo.Tags");
            DropForeignKey("dbo.TagCourses", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Courses", "Category_ID", "dbo.Categories");
            DropIndex("dbo.TagCourses", new[] { "Tag_ID" });
            DropIndex("dbo.TagCourses", new[] { "Course_ID" });
            DropIndex("dbo.Courses", new[] { "Category_ID" });
            DropIndex("dbo.Courses", new[] { "Author_ID" });
            DropTable("dbo.TagCourses");
            DropTable("dbo.Tags");
            DropTable("dbo.Categories");
            DropTable("dbo.Courses");
            DropTable("dbo.Authors");
        }
    }
}
