namespace Peah.YouHu.API.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PublishBindingModel
    {
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public string Destination { get; set; }

        [Required]
        [Display(Name = "Source")]
        public string Source { get; set; }

        [Required]
        [Display(Name = "Size")]
        public decimal Size { get; set; }

        [Required]
        [Display(Name = "Weight")]
        public decimal Weight { get; set; }

    }

    public class MakeDealBindingModel
    {
        [Required]
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Freight Unit Id")]
        public int FreightUnitId { get; set; }

        [Required]
        [Display(Name = "Modified Uesr")]
        public int ModifiedBy { get; set; }
    }

    public class ValidatePaymentCodeBindingModel
    {
        [Required]
        [Display(Name = "Driver Id")]
        public int DriverId { get; set; }

        [Required]
        [Display(Name = "Payment Code")]
        public string PaymentCode { get; set; }
    }

    public class PayBdingModel
    {
        [Required]
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Payment Code")]
        public string PaymentCode { get; set; }

        [Required]
        [Display(Name = "Freight Cost")]
        public decimal FreightCost { get; set; }

        //[Required]
        //[Display(Name = "Modified Uesr")]
        //public int ModifiedBy { get; set; }

        [Required]
        [Display(Name = "Payment Amount")]
        public decimal Paid { get; set; }
    }

    public class ConsignBindingModel
    {
        [Required]
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Payment Code")]
        public string PaymentCode { get; set; }

        [Required]
        [Display(Name = "Modified Uesr")]
        public int ModifiedBy { get; set; }
    }

    public class ConfirmDealBindingModel
    {
        //[Required]
        //[Display(Name = "Modified User")]
        //public int ModifiedBy { get; set; }

        [Required]
        [Display(Name = "Accepted Order Id")]
        public int AcceptedId { get; set; }

        [Required]
        [Display(Name = "Rejected Order Ids")]
        public IList<int> RejectedIds { get; set; }
    }

    public class UpdateOrderStateBindingModel
    {
        [Required]
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Order State")]
        public OrderState State { get; set; }

        //[Required]
        //[Display(Name = "Modified User")]
        //public int ModifiedBy { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }
    }
}