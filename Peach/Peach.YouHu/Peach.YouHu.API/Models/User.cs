namespace Peah.YouHu.API.Models
{
    using System;
    using System.Security.Principal;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public abstract class User : IdentityUser,IPrincipal, IUser
    {
        public string Password { get; set; }

        public string Name { get; set; }

        public string PaymentCode { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public int Rank { get; set; }

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
    }
}