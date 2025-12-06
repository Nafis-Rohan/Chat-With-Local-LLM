namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Intially : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Role = c.String(nullable: false, maxLength: 20, unicode: false),
                        Content = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 200, unicode: false),
                        Password = c.String(nullable: false, maxLength: 200, unicode: false),
                        Name = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        TKey = c.String(nullable: false, maxLength: 200, unicode: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ExpireAt = c.DateTime(),
                        UserName = c.String(nullable: false, maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.TKey);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatMessages", "UserId", "dbo.Users");
            DropIndex("dbo.ChatMessages", new[] { "UserId" });
            DropTable("dbo.Tokens");
            DropTable("dbo.Users");
            DropTable("dbo.ChatMessages");
        }
    }
}
