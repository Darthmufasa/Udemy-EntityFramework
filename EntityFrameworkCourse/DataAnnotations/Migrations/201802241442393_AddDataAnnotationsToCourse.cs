namespace DataAnnotations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataAnnotationsToCourse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "Author_Id1", "dbo.Authors");
            DropIndex("dbo.Courses", new[] { "Author_Id1" });
            RenameColumn(table: "dbo.Courses", name: "Author_Id1", newName: "AuthorId");
            AlterColumn("dbo.Courses", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Courses", "Description", c => c.String(nullable: false, maxLength: 2000));
            AlterColumn("dbo.Courses", "AuthorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Courses", "AuthorId");
            AddForeignKey("dbo.Courses", "AuthorId", "dbo.Authors", "Id", cascadeDelete: true);
            DropColumn("dbo.Courses", "Author_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Author_Id", c => c.Int());
            DropForeignKey("dbo.Courses", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Courses", new[] { "AuthorId" });
            AlterColumn("dbo.Courses", "AuthorId", c => c.Int());
            AlterColumn("dbo.Courses", "Description", c => c.String());
            AlterColumn("dbo.Courses", "Name", c => c.String());
            RenameColumn(table: "dbo.Courses", name: "AuthorId", newName: "Author_Id1");
            CreateIndex("dbo.Courses", "Author_Id1");
            AddForeignKey("dbo.Courses", "Author_Id1", "dbo.Authors", "Id");
        }
    }
}
