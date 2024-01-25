namespace Libraries.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminTables",
                c => new
                    {
                        AdminID = c.Int(nullable: false, identity: true),
                        AdminName = c.String(),
                        Password = c.String(),
                        Is_Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AdminID);
            
            CreateTable(
                "dbo.SecurityQuestionTables",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.StudentTables",
                c => new
                    {
                        ID = c.String(),
                        Name = c.String(),
                        Age = c.Int(nullable: false),
                        Address = c.String(),
                        Is_Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.UserLoginAndSecurityTables",
                c => new
                {
                    UserID = c.String(),
                    UserName = c.String(),
                    Password = c.String(),
                    QuestionCode = c.Int(nullable: false),
                    SecurityAnswer = c.String(),
                    Is_Active = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.UserID);
             
            
        }
        
        public override void Down()
        {
            
            DropTable("dbo.UserLoginAndSecurityTables");
            DropTable("dbo.StudentTables");
            DropTable("dbo.SecurityQuestionTables");
            DropTable("dbo.AdminTables");
        }
    }
}
