using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Models
{
    using System.ComponentModel.DataAnnotations;

    using Peah.YouHu.API.Models.Enum;
    public class Evaluation
    {
        [Key]
        public int Id { get; set; }

        public string Comments { get; set; }

        public EvaluationFrom From { get; set; }

        public Order Order { get; set; }

        public int Rank { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedBy { get; set; }
    }
}