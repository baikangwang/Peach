﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peah.YouHu.API.Models
{
    public interface IUser
    {
        int Id { get; set; }
        string Password { get; set; }
        string UserName { get; set; }
        string Name { get; set; }
        string PaymentCode { get; set; }

        int ModifiedBy { get; set; }

        DateTime ModifiedDate { get; set; }
    }
}