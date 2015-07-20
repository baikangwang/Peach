using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Models
{
    using System.ComponentModel.DataAnnotations;

    public abstract class AuditObj
    {
        [MaxLength(128)]
        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}