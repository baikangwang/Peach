namespace Peah.YouHu.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePassword : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Password", c => c.String(maxLength: 6));
        }
    }
}
