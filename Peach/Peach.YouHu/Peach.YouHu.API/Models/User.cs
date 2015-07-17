using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser, IUser
    {
        public string Password { get; set; }

        public string Name { get; set; }

        public string PaymentCode { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public int Rank { get; set; }
    }
}