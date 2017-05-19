namespace GreentubeGameTask.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        OverAllRate = c.Double(),
                        NumberOfVotes = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UsersGamesCommentsRates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        GameId = c.Long(nullable: false),
                        Comment = c.String(),
                        Rate = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Games", t => t.GameId)
                .Index(t => new { t.UserId, t.GameId }, unique: true, name: "UC_UsersGamesComments");
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "UC_User_Name");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersGamesCommentsRates", "GameId", "dbo.Games");
            DropForeignKey("dbo.UsersGamesCommentsRates", "UserId", "dbo.Users");
            DropIndex("dbo.Users", "UC_User_Name");
            DropIndex("dbo.UsersGamesCommentsRates", "UC_UsersGamesComments");
            DropTable("dbo.Users");
            DropTable("dbo.UsersGamesCommentsRates");
            DropTable("dbo.Games");
        }
    }
}
