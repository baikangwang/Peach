namespace Peah.YouHu.API.Models
{
    using System;

    public interface IUser
    {
        string Password { get; set; }
        string UserName { get; set; }
        string Name { get; set; }
        string PaymentCode { get; set; }

        string ModifiedBy { get; set; }

        DateTime ModifiedDate { get; set; }

        string Address { get; set; }

        string Phone { get; set; }

        int Rank { get; set; }
    }
}
