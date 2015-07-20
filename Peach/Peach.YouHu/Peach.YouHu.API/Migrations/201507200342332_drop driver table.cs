namespace Peah.YouHu.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropdrivertable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Drivers");
        }
        
        public override void Down()
        {
        }
    }
}
