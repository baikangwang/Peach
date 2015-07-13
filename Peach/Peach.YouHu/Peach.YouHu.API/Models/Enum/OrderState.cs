using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Models
{
    public enum OrderState
    {
        Ready,
        Dealing,
        Rejected,
        Dealt,
        Paying,
        Paid,
        InProgress,
        Arrived,
        Consigned
    }
}