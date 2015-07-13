using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Models
{
    using System.ComponentModel.DataAnnotations;

    using Peah.YouHu.API.Models.Enum;

    public class FreightUnit
    {
        [Key]
        public int Id { get; set; }

        public Driver Driver { get; set; }

        public float Height { get; set; }

        public float Length { get; set; }

        public string Licence { get; set; }

        public string Location { get; set; }

        public int? Rank { get; set; }

        public FreightUnitState State { get; set; }

        public FreightUnitType Type { get; set; }

        public float Weight { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}