namespace Libraries.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserLoginAndSecurityTables", "UserID", "dbo.StudentTables");
            DropIndex("dbo.UserLoginAndSecurityTables", new[] { "UserID" });
            DropPrimaryKey("dbo.StudentTables");
            DropPrimaryKey("dbo.UserLoginAndSecurityTables");
            AlterColumn("dbo.StudentTables", "ID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.UserLoginAndSecurityTables", "UserID", c => c.String(nullable: false, maxLength: 128));
            // Assuming ID in StudentTables is of type int
            AlterColumn("dbo.StudentTables", "ID", c => c.Int(nullable: false));
            // Assuming UserID in UserLoginAndSecurityTables is of type int
            AlterColumn("dbo.UserLoginAndSecurityTables", "UserID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLoginAndSecurityTables", "UserID", "dbo.StudentTables");
            DropIndex("dbo.UserLoginAndSecurityTables", new[] { "UserID" });
            DropPrimaryKey("dbo.UserLoginAndSecurityTables");
            DropPrimaryKey("dbo.StudentTables");

            // Restore the original data types
            AlterColumn("dbo.UserLoginAndSecurityTables", "UserID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.StudentTables", "ID", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
