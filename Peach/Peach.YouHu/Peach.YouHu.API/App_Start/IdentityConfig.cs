using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Peah.YouHu.API.Models;

namespace Peah.YouHu.API
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager
    {
        public static OwnerManager Create(IdentityFactoryOptions<OwnerManager> options, IOwinContext context)
        {
            var manager = new OwnerManager(new UserStore<Owner>(context.Get<OwnerDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<Owner>(manager)
                                    {
                                        AllowOnlyAlphanumericUserNames = false,
                                        RequireUniqueEmail = true
                                    };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
                                        {
                                            RequiredLength = 6,
                                            RequireNonLetterOrDigit = true,
                                            RequireDigit = true,
                                            RequireLowercase = true,
                                            RequireUppercase = true,
                                        };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<Owner>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
        public static DriverManager Create(IdentityFactoryOptions<DriverManager> options, IOwinContext context)
        {
            var manager = new DriverManager(new UserStore<Driver>(context.Get<DriverDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<Driver>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<Driver>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class OwnerManager : UserManager<Owner>
    {
        public OwnerManager(IUserStore<Owner> store)
            : base(store)
        {
        }
    }

    public class DriverManager : UserManager<Driver>
    {
        public DriverManager(IUserStore<Driver> store)
            : base(store)
        {
        }
    }
}
