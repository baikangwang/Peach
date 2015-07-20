namespace Peah.YouHu.API.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Order:AuditObj
    {
        [Key]
        public int Id { get; set; }

        public int? AlertCount { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string Destination { get; set; }

        public Evaluation DriverEvaluation { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal? FreightCost { get; set; }

        public FreightUnit FreightUnit { get; set; }

        public AppUser Owner { get; set; }

        public Evaluation OwnerEvaluation { get; set; }

        public decimal Size { get; set; }

        [MaxLength(200)]
        public string Source { get; set; }

        public decimal Weight { get; set; }

        public OrderState State { get; set; }

        public DateTime PublishedDate { get; set; }

        public decimal? Paid { get; set; }
    }
}