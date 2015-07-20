namespace Peah.YouHu.API.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class AppUser : IdentityUser,IPrincipal, IUser
    {
        [MaxLength(20)]
        public string FullName { get; set; }

        [MaxLength(6)]
        public string PaymentCode { get; set; }

        [MaxLength(128)]
        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        public int Rank { get; set; }

        public AppRole Role { get; set; }

        public bool IsInRole(string role)
        {
            return true;
        }

        private IIdentity _identity;
        public IIdentity Identity {
            get
            {
                return this._identity
                       ?? (this._identity =
                           new GenericIdentity(this.UserName, DefaultAuthenticationTypes.ExternalCookie));
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            AppUserManager manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}