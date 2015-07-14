using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peah.YouHu.API.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int? AlertCount { get; set; }

        public string Description { get; set; }

        public string Destination { get; set; }

        public Evaluation DriverEvaluation { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal? FreightCost { get; set; }

        public FreightUnit FreightUnit { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedBy { get; set; }

        public Owner Owner { get; set; }

        public Evaluation OwnerEvaluation { get; set; }

        public float Size { get; set; }

        public string Source { get; set; }

        public float Weight { get; set; }

        public OrderState State { get; set; }

        public DateTime PublishedDate { get; set; }

        public decimal? Paid { get; set; }
    }
}