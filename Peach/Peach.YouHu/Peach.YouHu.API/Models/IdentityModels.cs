namespace Peah.YouHu.API.Models
{
    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    //public class ApplicationUser : IdentityUser
    //{
    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }
    //}

    public class OwnerDbContext : IdentityDbContext<Owner>
    {
        public OwnerDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static OwnerDbContext Create()
        {
            return new OwnerDbContext();
        }

        //public System.Data.Entity.DbSet<Peah.YouHu.API.Models.Owner> Owners { get; set; }

        public DbSet<Driver> Drivers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Evaluation> Evaluations { get; set; }

        public DbSet<FreightUnit> FreightUnits { get; set; }
    }

    public class DriverDbContext : IdentityDbContext<Driver>
    {
        public DriverDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static DriverDbContext Create()
        {
            return new DriverDbContext();
        }

        public DbSet<Owner> Owners { get; set; }

        //public System.Data.Entity.DbSet<Peah.YouHu.API.Models.Driver> Drivers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Evaluation> Evaluations { get; set; }

        public DbSet<FreightUnit> FreightUnits { get; set; }
    }

    //public class OwnerDbContext : ApplicationDbContext,IdentityDbContext<Owner>
    //{

    //}
}