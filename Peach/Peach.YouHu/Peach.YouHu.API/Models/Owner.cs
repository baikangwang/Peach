using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Owner:IUser
    {
        [Key]
        public int Id { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public string PaymentCode { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Phone { get; set; }

        public int Rank { get; set; }
    }
}