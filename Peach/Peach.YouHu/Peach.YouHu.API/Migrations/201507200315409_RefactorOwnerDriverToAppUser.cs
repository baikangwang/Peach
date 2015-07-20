namespace Peah.YouHu.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    using Microsoft.Ajax.Utilities;

    public partial class RefactorOwnerDriverToAppUser : DbMigration
    {
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Evaluations", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "OwnerEvaluation_Id", "dbo.Evaluations");
            DropForeignKey("dbo.Orders", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "FreightUnit_Id", "dbo.FreightUnits");
            DropForeignKey("dbo.FreightUnits", "Driver_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "DriverEvaluation_Id", "dbo.Evaluations");
            DropForeignKey("dbo.DriverExts", "Driver_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.FreightUnits", new[] { "Driver_Id" });
            DropIndex("dbo.Orders", new[] { "OwnerEvaluation_Id" });
            DropIndex("dbo.Orders", new[] { "Owner_Id" });
            DropIndex("dbo.Orders", new[] { "FreightUnit_Id" });
            DropIndex("dbo.Orders", new[] { "DriverEvaluation_Id" });
            DropIndex("dbo.Evaluations", new[] { "Order_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.DriverExts", new[] { "Driver_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.FreightUnits");
            DropTable("dbo.Orders");
            DropTable("dbo.Evaluations");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.DriverExts");
        }

        public void Cleanup()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Evaluations", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "OwnerEvaluation_Id", "dbo.Evaluations");
            DropForeignKey("dbo.Orders", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "FreightUnit_Id", "dbo.FreightUnits");
            DropForeignKey("dbo.FreightUnits", "Driver_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "DriverEvaluation_Id", "dbo.Evaluations");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.FreightUnits", new[] { "Driver_Id" });
            DropIndex("dbo.Orders", new[] { "OwnerEvaluation_Id" });
            DropIndex("dbo.Orders", new[] { "Owner_Id" });
            DropIndex("dbo.Orders", new[] { "FreightUnit_Id" });
            DropIndex("dbo.Orders", new[] { "DriverEvaluation_Id" });
            DropIndex("dbo.Evaluations", new[] { "Order_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.FreightUnits");
            DropTable("dbo.Orders");
            DropTable("dbo.Evaluations");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
        }

        public override void Up()
        {
            this.Cleanup();

            CreateTable(
                "dbo.DriverExts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentIncome = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalIncome = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedDate = c.DateTime(nullable: false),
                        Driver_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Driver_Id)
                .Index(t => t.Driver_Id);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Password = c.String(maxLength: 6),
                        FullName = c.String(maxLength: 20),
                        PaymentCode = c.String(maxLength: 6),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedDate = c.DateTime(nullable: false),
                        Address = c.String(maxLength: 200),
                        Rank = c.Int(nullable: false),
                        Role = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Evaluations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comments = c.String(maxLength: 500),
                        From = c.Int(nullable: false),
                        Rank = c.Int(nullable: false),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedDate = c.DateTime(nullable: false),
                        Order_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AlertCount = c.Int(),
                        Description = c.String(maxLength: 100),
                        Destination = c.String(maxLength: 200),
                        DueDate = c.DateTime(),
                        FreightCost = c.Decimal(precision: 18, scale: 2),
                        Size = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Source = c.String(maxLength: 200),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        State = c.Int(nullable: false),
                        PublishedDate = c.DateTime(nullable: false),
                        Paid = c.Decimal(precision: 18, scale: 2),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedDate = c.DateTime(nullable: false),
                        DriverEvaluation_Id = c.Int(),
                        FreightUnit_Id = c.Int(),
                        Owner_Id = c.String(maxLength: 128),
                        OwnerEvaluation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Evaluations", t => t.DriverEvaluation_Id)
                .ForeignKey("dbo.FreightUnits", t => t.FreightUnit_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .ForeignKey("dbo.Evaluations", t => t.OwnerEvaluation_Id)
                .Index(t => t.DriverEvaluation_Id)
                .Index(t => t.FreightUnit_Id)
                .Index(t => t.Owner_Id)
                .Index(t => t.OwnerEvaluation_Id);
            
            CreateTable(
                "dbo.FreightUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Height = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Length = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Licence = c.String(maxLength: 10),
                        Location = c.String(maxLength: 50),
                        Rank = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedDate = c.DateTime(nullable: false),
                        Driver_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Driver_Id)
                .Index(t => t.Driver_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
    }
}
