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

    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        public DbSet<DriverExt> DriverExts { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Evaluation> Evaluations { get; set; }

        public DbSet<FreightUnit> FreightUnits { get; set; }
    }
}