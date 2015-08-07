namespace Peah.YouHu.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedConsignCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ConsignCode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ConsignCode");
        }
    }
}
