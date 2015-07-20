namespace Peah.YouHu.API.Models
{
    using System;

    public interface IUser
    {
        string UserName { get; set; }
        string FullName { get; set; }
        string PaymentCode { get; set; }

        string ModifiedBy { get; set; }

        DateTime ModifiedDate { get; set; }

        string Address { get; set; }

        int Rank { get; set; }

        AppRole Role { get; set; }
    }
}
