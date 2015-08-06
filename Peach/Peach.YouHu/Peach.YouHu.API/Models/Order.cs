namespace Peah.YouHu.API.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;
    using System.Runtime.Serialization;

    using Microsoft.Ajax.Utilities;

    public class Order:AuditObj
    {
        [Key]
        public int Id { get; set; }

        public int? AlertCount { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string Destination { get; set; }

        public virtual Evaluation DriverEvaluation { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal? FreightCost { get; set; }

        public virtual FreightUnit FreightUnit { get; set; }

        public virtual AppUser Owner { get; set; }

        public virtual Evaluation OwnerEvaluation { get; set; }

        public decimal Size { get; set; }

        [MaxLength(200)]
        public string Source { get; set; }

        public decimal Weight { get; set; }

        public OrderState State { get; set; }

        public DateTime PublishedDate { get; set; }

        public decimal? Paid { get; set; }

        public int ConsignCode { get; set; }
    }

    //public class OrderMapper : EntityTypeConfiguration<Order>
    //{
    //    public OrderMapper()
    //    {
    //        this.ToTable("Order", "Order")
    //            .HasKey(c=>c.Id);
    //            this.Property(c=>c.AlertCount).IsOptional();
    //            this.Property(c => c.Description).IsRequired().HasMaxLength(200);
    //            this.Property(c => c.Destination).IsRequired();
    //            this.Property(c => c.AlertCount).IsOptional();
    //            this.Property(c => c.AlertCount).IsOptional();
    //            this.Property(c => c.AlertCount).IsOptional();
    //            this.Property(c => c.AlertCount).IsOptional();
    //            this.Property(c => c.AlertCount).IsOptional();
    //            this.Property(c => c.AlertCount).IsOptional();
    //            this.Property(c => c.AlertCount).IsOptional();
    //            this.Property(c => c.AlertCount).IsOptional();

    //    }
    //}
}