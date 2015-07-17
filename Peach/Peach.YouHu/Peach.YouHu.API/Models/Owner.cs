using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class Owner:User
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            OwnerManager manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}