﻿namespace Peah.YouHu.API
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;

    using Peah.YouHu.API.Models;

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class AppUserManager:UserManager<AppUser>
    {
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(new UserStore<AppUser>(context.Get<AppDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<AppUser>(manager)
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
                manager.UserTokenProvider = new DataProtectorTokenProvider<AppUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public AppUserManager(IUserStore<AppUser> store)
            : base(store)
        {
        }
    }
}
