using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class Driver:User
    {
        public List<FreightUnit> FreightUnits { get; set; }

        public decimal CurrentIncome { get; set; }

        public decimal TotalIncome { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            DriverManager manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}